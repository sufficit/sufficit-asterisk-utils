using System;
using System.Linq;

namespace Sufficit.Asterisk
{
    public static partial class Utils
    {
        public static string[] TRUEVALUES = { "yes", "true", "on", "1", "ok" };
        public static string[] FALSEVALUES = { "no", "false", "off", "0", "non" };
        public static string[] NULLVALUES = { "null", "" };

        /// <summary>
        /// Converting nullable string from asterisk format to boolean
        /// </summary>
        public static bool? ToBoolean(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var test = value!.Trim().ToLowerInvariant();
            if (NULLVALUES.Contains(test)) return null;
            if (TRUEVALUES.Contains(test)) return true;
            if (FALSEVALUES.Contains(test)) return false;

            throw new Exception($"value was not recognized as boolean: {value}");
        }

        /// <summary>
        /// Converting boolean to asterisk compatible format nullable string
        /// </summary>
        public static string? ToAsteriskString(bool? value)
        {
            if (!value.HasValue) return null;
            return value.Value ? TRUEVALUES[0] : FALSEVALUES[0];
        }

        /// <summary>
        ///     Get the middle part of a channel name, between @ and /, first of -
        /// </summary>
        /// <remarks>
        ///     <para>ex: "LOCAL/2344534fgdfgdfg-4543611@from-queues" => 2344534fgdfgdfg</para>
        ///     <para>ex: "PJSIP/out-datora-4543611@external" => out-datora</para>
        /// </remarks>
        public static string GetChannelTitle (string? channel)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(channel))
            {
                result = channel;

                if (result.Contains('@'))
                    result = result.Split('@')[0];

                if (result.Contains('/'))
                {
                    var splitted = result.Split('/');
                    if (splitted.Length > 1)
                        result = splitted[1];
                }

                var last = result.LastIndexOf('-');
                if (last > -1)
                    result = result.Remove(last);
            }
            return result;
        }
    }
}