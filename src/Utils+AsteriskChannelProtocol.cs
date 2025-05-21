using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Sufficit.Asterisk
{
    public static partial class Utils
    {
        /// <summary>
        ///     Generate AsteriskChannel from string
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static AsteriskChannelProtocol ToAsteriskChannelProtocol(string source)
        {
            return source.ToUpperInvariant().Trim() switch
            {
                "LOCAL" => AsteriskChannelProtocol.LOCAL,
                "SIP" => AsteriskChannelProtocol.SIP,
                "PJSIP" => AsteriskChannelProtocol.PJSIP,
                "IAX" => AsteriskChannelProtocol.IAX,
                "IAX2" => AsteriskChannelProtocol.IAX2,
                "MESSAGE" => AsteriskChannelProtocol.MESSAGE,
                _ => throw new ArgumentOutOfRangeException(nameof(source), $"unrecognized protocol: {source}"),
            };
        }

        /// <summary>
        ///     <para>Try to parse asterisk channel protocol from string</para>
        ///     <para><see cref="ToAsteriskChannelProtocol(string)"/></para>
        /// </summary>
        public static bool TryParseAsteriskChannelProtocol(string source, out AsteriskChannelProtocol protocol)
        {            
            try
            {
                protocol = ToAsteriskChannelProtocol(source);
                return true;
            }
            catch {
                protocol = AsteriskChannelProtocol.UNKNOWN;
                return false;
            }   
        }
    }
}
