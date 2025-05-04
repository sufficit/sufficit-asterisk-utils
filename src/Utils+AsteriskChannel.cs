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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">for unrecognized channel protocol</exception>
        public static AsteriskChannel AsteriskChannelGenerate (string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            var channel = new AsteriskChannel
            {
                Id = source
            };

            if (channel.Id.Contains("/"))
            {
                var splitted = channel.Id.Split('/');
                var tech = splitted[0];
                channel.Protocol = ToAsteriskChannelProtocol(tech);

                var track = splitted[1];
                var separator = track.LastIndexOf('-');
                if (separator > -1)
                {
                    channel.Suffix = track.Substring(separator + 1, track.Length - (separator + 1));
                    channel.Name = track.Substring(0, separator);
                }
                else
                {
                    channel.Name = track;
                }

                if (channel.Name.Contains("@"))
                {
                    var withContext = channel.Name.Split('@');
                    if (withContext.Length >= 1)
                        channel.Context = withContext[1];

                    channel.Name = withContext[0];
                }
            }

            return channel;
        }

        /// <summary>
        ///     Generate AsteriskChannel from <see cref="PeerInfo"/>
        /// </summary>
        public static AsteriskChannel AsteriskChannelGenerate(PeerInfo source)
            => new AsteriskChannel
            {
                Id = $"{source.Protocol}/{source.Name}",
                Protocol = source.Protocol,
                Name = source.Name
            };        
    }
}
