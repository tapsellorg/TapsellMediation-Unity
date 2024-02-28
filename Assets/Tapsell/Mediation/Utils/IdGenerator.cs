using System;
using System.Linq;

namespace Tapsell.Mediation.Utils
{
    internal static class IdGenerator
    {
        private static readonly Random Random = new Random();

        public static string GenerateId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}