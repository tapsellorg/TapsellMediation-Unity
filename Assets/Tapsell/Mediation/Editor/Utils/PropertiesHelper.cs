using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tapsell.Mediation.Editor.Utils
{
    /// <summary>
    /// Loader for <c>properties</c> files. See the
    /// <a href="https://docs.oracle.com/javase/9/docs/api/java/util/Properties.html#load-java.io.Reader-">description</a>
    /// of <c>java.util.Properties.load(java.io.Reader)</c> about
    /// the format of <c>properties</c> files.
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// using (TextReader reader = File.OpenText("test.properties"))
    /// {
    ///     IDictionary&lt;string, string&gt; properties =
    ///         PropertiesLoader.Load(reader);
    /// }
    /// </code>
    /// </example>
    public static class PropertiesHelper
    {
        private static readonly Regex IgnorableLinePattern;
        private static readonly Regex ContinuingLinePattern;


        static PropertiesHelper()
        {
            // Regular expression for ignorable lines (blank lines and comment lines).
            IgnorableLinePattern = new Regex("^[ \t\f]*([#!].*)?$", RegexOptions.Compiled);

            // Regular expression for line whose line terminator is escaped by a backslash.
            //
            //   From java.util.Properties.load(java.io.Reader)
            //
            // ... Note that it is not sufficient to only examine the character preceding a line
            // terminator sequence to decide if the line terminator is escaped; there must be
            // an odd number of contiguous backslashes for the line terminator to be escaped.
            // Since the input is processed from left to right, a non-zero even number of 2n
            // contiguous backslashes before a line terminator (or elsewhere) encodes n backslashes
            // after escape processing.
            ContinuingLinePattern = new Regex(@"(^|.*[^\\])(\\\\)*\\$", RegexOptions.Compiled);
        }


        /// <summary>
        /// Extract key-value pairs from a reader whose content complies with the specification of
        /// the <c>properties</c> file. See the
        /// <a href="https://docs.oracle.com/javase/9/docs/api/java/util/Properties.html#load-java.io.Reader-">description</a>
        /// of <c>java.util.Properties.load(java.io.Reader)</c> about the format of <c>properties</c> files.
        /// </summary>
        ///
        /// <returns>
        /// Key-value pairs extracted from the reader.
        /// </returns>
        ///
        /// <param name="reader">
        /// A reader whose content complies with the specification of the <c>properties</c> file.
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// A malformed sequence was found.
        /// </exception>
        public static IDictionary<string, string> Load(TextReader reader)
        {
            var properties = new Dictionary<string, string>();

            while (true)
            {
                // Read an effective line.
                var line = ReadEffectiveLine(reader);

                if (line == null)
                {
                    // No more effective line.
                    break;
                }

                // Split the line into a key and a value.
                var pair = ParseLine(line);

                // If the line could not be parsed correctly.
                if (pair == null)
                {
                    continue;
                }

                // Succeeded in extracting a key-value pair.
                properties.Add(pair.Value.Key, pair.Value.Value);
            }

            return properties;
        }

        public static void Write(IDictionary<string, string> properties, TextWriter writer)
        {
            foreach(var (key, value) in properties)
            {
                // print the key and value
                // Debug.LogError("key: " + key + ", value: " + value);
                writer.Write(key + "=" + value + "\n");
            }
        }


        private static string ReadEffectiveLine(TextReader reader)
        {
            // Skip blank lines and comment lines.
            var line = SkipIgnorableLines(reader);

            if (line == null)
            {
                // No more line.
                return null;
            }

            // Remove leading white spaces if any.
            line = RemoveLeadingWhiteSpaces(line);

            // If the line terminator is not escaped by '\'.
            if (ContinuingLinePattern.IsMatch(line) == false)
            {
                // No need to concatenate the next line.
                return line;
            }

            var builder = new StringBuilder();

            // Remove the backslash at the end and append the resultant string.
            builder.Append(RemoveLastChar(line));

            while (true)
            {
                line = reader.ReadLine();

                // If the end of the stream was reached.
                if (line == null)
                {
                    break;
                }

                // Ignore leading white spaces as the spec requires.
                line = RemoveLeadingWhiteSpaces(line);

                // If the line terminator is not escaped by '\'.
                if (ContinuingLinePattern.IsMatch(line) == false)
                {
                    // Append the line as is.
                    builder.Append(line);

                    // And this is the end of the logical line.
                    break;
                }

                // Remove the backslash at the end and append the
                // resultant string.
                builder.Append(RemoveLastChar(line));
            }

            return builder.ToString();
        }

        private static string SkipIgnorableLines(TextReader reader)
        {
            while (true)
            {
                // Read one line.
                var line = reader.ReadLine();

                // If the end of the stream was reached.
                if (line == null)
                {
                    // No more line.
                    return null;
                }

                // If the line is ignorable.
                if (IgnorableLinePattern.IsMatch(line))
                {
                    // Skip the line.
                    continue;
                }

                // Found a non-ignorable line.
                return line;
            }
        }

        private static string RemoveLeadingWhiteSpaces(string str)
        {
            return str.TrimStart(' ', '\t', '\f');
        }

        private static string RemoveLastChar(string str)
        {
            return str[..^1];
        }

        private static KeyValuePair<string, string>? ParseLine(string line)
        {
            // The index which points to the beginning of the value.
            var index = 0;

            // Extract the key. 'index' will be updated and it will point to the beginning of the value.
            var key = ExtractKey(line, ref index);

            // If the key is an empty string.
            if (key.Length == 0)
            {
                // No valid key was found.
                return null;
            }

            // Extract the value.
            var value = ExtractValue(line, index);

            // Extracted a key-value pair from the line.
            return new KeyValuePair<string, string>(key, value);
        }

        private static string ExtractKey(string line, ref int index)
        {
            // From java.util.Properties.load(java.io.Reader)
            //
            //   The key contains all of the characters in the line starting with the first non-whitespace
            //   character and up to, but not including, the first unescaped '=', ':', or whitespace
            //   character other than a line terminator. All of these key termination characters
            //   may be included in the key by escaping them with a preceding backslash character;
            //   for example,
            //   \:\=
            //   would be the two-character key ":=". Line terminator characters can be included
            //   using \r and \n escape sequences. Any white space after the key is skipped;
            //   if the first non-whitespace character after the key is '=' or ':', then it is
            //   ignored and any whitespace characters after it are also skipped. All remaining
            //   characters on the line become part of the associated element string;
            //   if there are no remaining characters, the element is the empty string "".
            //   Once the raw character sequences constituting the key and element are
            //   identified, escape processing is performed as described above.

            var builder = new StringBuilder();

            var len = line.Length;

            for (var i = 0; i < len; ++i)
            {
                var c = line[i];

                // If a separator was found.
                if (IsSeparator(c))
                {
                    // The end of the key was reached.
                    // Make 'index' point to the value.
                    OnSeparatorFound(line, ref index, i);
                    break;
                }

                // If a white space was found.
                if (IsWhiteSpace(c))
                {
                    // The end of the key was reached.
                    // Make 'index' point to the value.
                    OnWhiteSpaceFound(line, ref index, i);
                    break;
                }

                // If a backslash was found.
                if (c == '\\')
                {
                    // Interpret the special sequence and increment 'i' as necessary.
                    OnBackSlashFound(line, ref i, builder);
                }
                else
                {
                    // Append the character as is.
                    builder.Append(c);
                }

                // If the end of the line was reached.
                if (i + 1 == len)
                {
                    // No value.
                    index = len;
                    break;
                }
            }

            // Build the key.
            return builder.ToString();
        }

        private static void OnSeparatorFound(string line, ref int index, int i)
        {
            // Skip whitespaces which may follow the separator.
            // 'index' will point to the beginning of the value.
            index = SkipWhiteSpaces(line, i + 1);
        }

        private static void OnWhiteSpaceFound(string line, ref int index, int i)
        {
            // Skip white spaces which may follow the key. 'index' will point to (1) the separator
            // or (2) the beginning of the value.
            index = SkipWhiteSpaces(line, i + 1);

            // If 'index' points to the separator.
            if ((index < line.Length) && IsSeparator(line[index]))
            {
                // Skip white spaces which may follow the separator.
                // 'index' will point to the beginning of the value.
                index = SkipWhiteSpaces(line, index + 1);
            }
        }

        private static void OnBackSlashFound(string line, ref int i, StringBuilder builder)
        {
            // In this context, it is assured that there exists a character after the backslash
            // (= the backslash is not the last character of the string).
            var c = line[++i];

            switch (c)
            {
                default:  builder.Append(c);    return;
                case 'f': builder.Append('\f'); return;
                case 'n': builder.Append('\n'); return;
                case 'r': builder.Append('\r'); return;
                case 't': builder.Append('\t'); return;
                case 'u': break;
            }

            // '\uxxxx' is expected where 'x' is [0-9A-Fa-f].
            // Convert 'xxxx' into an integer.
            var n = ReadFourDigitHex(line, i + 1);

            if (n == null)
            {
                throw new ArgumentException("Malformed \\uxxxx encoding.");
            }

            builder.Append((char)n);
            i += 4;
        }

        private static int SkipWhiteSpaces(string str, int i)
        {
            var len = str.Length;

            for ( ; i < len; ++i)
            {
                if (IsWhiteSpace(str[i]) == false)
                {
                    break;
                }
            }

            return i;
        }

        private static int? ReadFourDigitHex(string str, int start)
        {
            if (str.Length <= start + 3)
            {
                return null;
            }

            var i0 = HexToInt(str[start    ]);
            var i1 = HexToInt(str[start + 1]);
            var i2 = HexToInt(str[start + 2]);
            var i3 = HexToInt(str[start + 3]);

            if (i0 == null || i1 == null || i2 == null || i3 == null)
            {
                return null;
            }

            return (i0.Value << 12) |
                   (i1.Value <<  8) |
                   (i2.Value <<  4) |
                   i3.Value;
        }

        private static string ExtractValue(string line, int index)
        {
            var len = line.Length;
            var builder = new StringBuilder();

            for (var i = index; i < len; ++i)
            {
                var c = line[i];

                // If the character is not a backslash.
                if (c != '\\')
                {
                    // Append the character as is.
                    builder.Append(c);
                    continue;
                }

                // Interpret the sequence.
                OnBackSlashFound(line, ref i, builder);
            }

            // Build a value.
            return builder.ToString();
        }

        private static bool IsWhiteSpace(char c)
        {
            return c is ' ' or '\t' or '\f';
        }

        private static bool IsSeparator(char c)
        {
            return c is '=' or ':';
        }

        /// <summary>
        /// Convert a character which represents a hex digit (<c>0-9</c>, <c>A-F</c>, <c>a-f</c>)
        /// to an integer (from 0 to 15). If the given character is not a hex digit, <c>null</c> is returned.
        /// </summary>
        ///
        /// <param name="c">
        /// A hex digit (<c>0-9</c>, <c>A-F</c>, <c>a-f</c>).
        /// </param>
        private static int? HexToInt(char c)
        {
            // 0-9
            if (c is >= '0' and <= '9')
            {
                return c - '0';
            }

            // A-F
            if (c is >= 'A' and <= 'F')
            {
                return c - 'A' + 10;
            }

            // a-f
            if (c is >= 'a' and <= 'f')
            {
                return c - 'a' + 10;
            }

            // Invalid character.
            return null;
        }
    }
}