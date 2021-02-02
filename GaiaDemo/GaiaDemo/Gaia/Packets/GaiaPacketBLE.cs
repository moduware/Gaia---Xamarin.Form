using System;

namespace GaiaDemo.Gaia.Packet
{
    /// <summary>
    /// This class encapsulates a Gaia packet sent and received over a Bluetooth Low Energy connection with a BLE device.
    /// The packet encapsulated by this class is represented by the following byte structure: 
    /// 0 bytes  1         2        3         4         5       len+4
    /// +--------+---------+--------+--------+ +--------+--------+
    /// |    VENDOR ID     |   COMMAND ID    | | PAYLOAD   ...   |
    /// +--------+---------+--------+--------+ +--------+--------+
    /// </summary>
    public class GaiaPacketBLE : GaiaPacket
    {
        /// <summary>
        /// The maximum length for the packet payload.
        /// The BLE data length maximum for a packet is 20.
        /// </summary>
        public const int MAX_PAYLOAD = 16;
        // The offset for the bytes which represents the vendor id in the byte structure.
        private const int OFFSET_VENDOR_ID = 0;
        // The number of bytes which represents the vendor id in the byte structure.
        private const int LENGTH_VENDOR_ID = 2;
        // The offset for the bytes which represents the command id in the byte structure.
        private const int OFFSET_COMMAND_ID = 2;
        // The number of bytes which represents the command id in the byte structure.
        private const int LENGTH_COMMAND_ID = 2;
        // The offset for the bytes which represents the payload in the byte structure.
        private const int OFFSET_PAYLOAD = 4;
        /// <summary>
        /// The number of bytes which contains the information to identify the type of packet.
        /// </summary>
        public const int PACKET_INFORMATION_LENGTH = LENGTH_COMMAND_ID + LENGTH_VENDOR_ID;
        /// <summary>
        /// The minimum length of a packet.
        /// </summary>
        public const int MIN_PACKET_LENGTH = PACKET_INFORMATION_LENGTH;

        /// <summary>
        /// Constructor that builds a packet from a byte sequence.
        /// </summary>
        /// <param name="source">Array of bytes to build the command from.</param>
        public GaiaPacketBLE(byte[] source)
        {
            int payloadLength = source.Length - PACKET_INFORMATION_LENGTH;

            if (payloadLength < 0)
            {
                throw new GaiaException(GaiaException.Type.PACKET_PAYLOAD_INVALID_PARAMETER);
            }

            VendorId = GaiaUtils.ExtractIntFromByteArray(source, OFFSET_VENDOR_ID, LENGTH_VENDOR_ID, false);
            CommandId = GaiaUtils.ExtractIntFromByteArray(source, OFFSET_COMMAND_ID, LENGTH_COMMAND_ID, false);

            if (payloadLength > 0)
            {
                Payload = new byte[payloadLength];
                Array.Copy(source, OFFSET_PAYLOAD, Payload, 0, payloadLength);
            }

            Bytes = source;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which are parts of a packet.
        /// </summary>
        /// <param name="_vendorId">the vendor ID of the packet.</param>
        /// <param name="_commandId">the command ID of the packet.</param>
        public GaiaPacketBLE(int _vendorId, int _commandId)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = new byte[0];
            Bytes = null;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which are parts of a packet.
        /// </summary>
        /// <param name="_vendorId">the vendor ID of the packet.</param>
        /// <param name="_commandId">the command ID of the packet.</param>
        /// <param name="_payload">the payload of the packet.</param>
        public GaiaPacketBLE(int _vendorId, int _commandId, byte[] _payload)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = _payload;
            Bytes = null;
        }

        /// <summary>
        /// To build the byte array which represents this Gaia Packet over BLE.
        /// The bytes array is built according to the definition of a GAIA Packet sent over BLE:
        /// 0 bytes  1         2        3         4                 len+4
        /// +--------+---------+--------+--------+ +--------+--------+
        /// |    VENDOR ID     |   COMMAND ID    | | PAYLOAD   ...   |
        /// +--------+---------+--------+--------+ +--------+--------+
        /// </summary>
        /// <param name="_commandId">The command ID for the packet bytes to build:
        /// The original command ID of this packet.
        /// The acknowledgement version of the original command ID.</param>
        /// <param name="_payload">The payload to include in the packet bytes.</param>
        /// <returns>A new byte array built with the given information.</returns>
        public override byte[] BuildBytes(int _commandId, byte[] _payload)
        {
            int length = _payload.Length + OFFSET_PAYLOAD;
            byte[] data = new byte[length];

            try
            {
                GaiaUtils.CopyIntIntoByteArray(VendorId, data, OFFSET_VENDOR_ID, LENGTH_VENDOR_ID, false);
                GaiaUtils.CopyIntIntoByteArray(_commandId, data, OFFSET_COMMAND_ID, LENGTH_COMMAND_ID, false);
                Array.Copy(_payload, 0, data, OFFSET_PAYLOAD, _payload.Length);
            }
            catch (GaiaException)
            {
                throw new GaiaException(GaiaException.Type.PAYLOAD_LENGTH_TOO_LONG);
            }            

            return data;
        }

        public override int GetPayloadMaxLength()
        {
            return MAX_PAYLOAD;
        }
    }
}
