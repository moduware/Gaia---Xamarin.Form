using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaDemo
{
    /// <summary>
    /// A GaiaException is thrown when a problem occurs with the GAIA protocol.
    /// </summary>
    public class GaiaException : Exception
    {        
        /// <summary>
        /// >The type of the gaia exception.
        /// </summary>
        public Type Error { get; set; }
        /// <summary>
        /// If the exception is linked to a command, the corresponding command.
        /// </summary>
        private int Command { get; set; }

        /// <summary>
        /// All types of gaia exceptions.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// This exception occurs when trying to build a GAIA packet but the given payload is longer than the maximum length the packet can be.
            /// </summary>
            PAYLOAD_LENGTH_TOO_LONG = 0,
            /// <summary>
            /// This exception occurs when trying to build an acknowledgement packet on an acknowledgement packet.
            /// </summary>
            PACKET_IS_ALREADY_AN_ACKNOWLEDGEMENT = 1,
            /// <summary>
            /// This exception occurs when trying to use a non notification packet as a notification packet.
            /// </summary>
            PACKET_NOT_A_NOTIFICATION = 2,
            /// <summary>
            /// This exception occurs when the payload does not contain the value requested for a command or when building a new GAIA packet and the given argument is incorrect.
            /// For example, the command EVENT_NOTIFICATION required the event code as the first byte of the payload.
            /// For example, we are trying to build a packet from a byte array which does not contain enough bytes.
            /// </summary>
            PACKET_PAYLOAD_INVALID_PARAMETER = 3,
            /// <summary>
            /// This exception occurs when trying to use a non acknowledgement packet as an acknowledgement packet.
            /// </summary>
            PACKET_NOT_AN_ACKNOWLEDGMENT = 4
        }

        /// <summary>
        /// Class constructor for this exception.
        /// </summary>
        /// <param name="_type">the type of this exception.</param>
        public GaiaException(Type _type)
        {
            Error = _type;
            Command = -1;
        }

        /// <summary>
        /// Class constructor for this exception.
        /// </summary>
        /// <param name="_type">the type of this exception.</param>
        /// <param name="_command">the command linked to the exception if occurred while working with a command.</param>
        public GaiaException(Type _type, int _command)
        {
            Error = _type;
            Command = _command;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            switch (Error)
            {
                case Type.PAYLOAD_LENGTH_TOO_LONG:
                    builder.Append("Build of a packet failed: the payload length is bigger than the authorized packet length.");
                    break;
                case Type.PACKET_IS_ALREADY_AN_ACKNOWLEDGEMENT:
                    builder.Append("Build of a packet failed: the packet is already an acknowledgement packet: not possible to create an acknowledgement packet from it.");
                    break;
                case Type.PACKET_NOT_A_NOTIFICATION:
                    builder.Append("Packet is not a COMMAND NOTIFICATION");
                    if (Command >= 0)
                    {
                        builder.Append(", received command: ");
                        builder.Append(GaiaUtils.GetCommandToString(Command));
                    }
                    break;
                case Type.PACKET_PAYLOAD_INVALID_PARAMETER:
                    builder.Append("Payload is missing argument");
                    if (Command >= 0)
                    {
                        builder.Append(" for command: ");
                        builder.Append(GaiaUtils.GetCommandToString(Command));
                    }
                    break;
                case Type.PACKET_NOT_AN_ACKNOWLEDGMENT:
                    builder.Append("The packet is not an acknowledgement, ");
                    if (Command >= 0)
                    {
                        builder.Append(" received command: ");
                        builder.Append(GaiaUtils.GetCommandToString(Command));
                    }
                    break;
                default:
                    builder.Append("Gaia Exception occurred.");
                    break;
            }
            return builder.ToString();
        }
    }
}
