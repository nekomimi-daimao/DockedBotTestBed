using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public static class ArgumentParser
    {
        public static Dictionary<string, string> Args = new();
        private const string Prefix = "-";

        [RuntimeInitializeOnLoadMethod]
        private static void Parse()
        {
            Args[nameof(IsBot)] = IsBot().ToString();
            Args = ParseArgs();
        }

        public static bool IsBot()
        {
            var isBot = Application.isBatchMode;
#if UNITY_SERVER
            isBot = true;
#endif
            return isBot;
        }

        private static Dictionary<string, string> ParseArgs()
        {
            var dic = new Dictionary<string, string>();

            var args = Environment.GetCommandLineArgs();
            for (var count = 0; count < args.Length; count++)
            {
                var key = args[count];
                if (!key.StartsWith(Prefix))
                {
                    continue;
                }

                string value = null;
                if (count < args.Length)
                {
                    value = args[count + 1];
                    if (value.StartsWith(Prefix))
                    {
                        value = null;
                    }
                }
                dic[key.Substring(1)] = value;
            }

            return dic;
        }
    }
}
