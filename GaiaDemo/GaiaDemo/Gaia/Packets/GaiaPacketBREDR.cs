using System;

namespace GaiaDemo.Gaia.Packet
{
    /// <summary>
    /// This class encapsulates a Gaia packet sent and received over a BR/EDR connection with a Bluetooth device.
    /// The packet encapsulated by this class is represented by the following byte structure over Bluetooth:
    /// 0 bytes  1         2        3        4        5        6        7         8         9       len+8
    /// +--------+---------+--------+--------+--------+--------+--------+--------+ +--------+--------+ +--------+
    /// |   SOF  | VERSION | FLAGS  | LENGTH |    VENDOR ID    |   COMMAND ID    | | PAYLOAD   ...   | | CHECK  |
    /// +--------+---------+--------+--------+--------+--------+--------+--------+ +--------+--------+ +--------+
    /// 
    /// Start of frame (SOF)    |   Length: 1 byte      |   Value: 0xFF
    /// Version                 |   Length: 1 byte      |   Value: 0x01 for first version of the protocol
    /// Flags                   |   Length: 1 byte      |   Bit 0: GAIA_FLAG_CHECK, checksum is in use      |   Bits 1 - 7: reserved, must be zero
    /// Payload Length          |   Length: 1 byte      |   A maximum payload length of 254 bytes reduces the likelihood of spurious SOFs in the packet
    /// Vendor ID               |   Length: 2 bytes     |   Bluetooth SIG already have assigned numbers identifying member companies. For instance, CSR is 0x000A.
    /// Command ID              |   Length: 2 bytes     |   See Commands for valid Command IDs
    /// Payload                 |   Length: As defined by 'Payload Length' header field                     |   Format implicit in COMMAND ID
    /// Checksum                |   Optional -- see Flags above             |   Length: 1 byte              |   Simple XOR of all bytes in the packet
    /// </summary>
    public class GaiaPacketBREDR : GaiaPacket
    {
        /// <summary>
        /// The maximum length of a complete packet.
        /// </summary>
        public const int MAX_PACKET = 263;
        /// <summary>
        /// The maximum length for the packet payload.
        /// </summary>
        public const int MAX_PAYLOAD = 254;
        /// <summary>
        /// The mask for the flag of this packet if requested.
        /// </summary>
        public const int FLAG_CHECK_MASK = 0x01;
        /// <summary>
        /// The offset for the bytes which represents the SOF - start of frame - in the byte structure.
        /// </summary>
        public const int OFFSET_SOF = 0;
        /// <summary>
        /// The offset for the bytes which represents the protocol version in the byte structure.
        /// </summary>
        public const int OFFSET_VERSION = 1;
        /// <summary>
        /// The offset for the bytes which represents the flag in the byte structure.
        /// </summary>
        public const int OFFSET_FLAGS = 2;
        /// <summary>
        /// The offset for the bytes which represents the payload length in the byte structure.
        /// </summary>
        public const int OFFSET_LENGTH = 3;
        /// <summary>
        /// The offset for the bytes which represents the vendor id in the byte structure.
        /// </summary>
        public const int OFFSET_VENDOR_ID = 4;
        /// <summary>
        /// The number of bytes which represents the vendor id in the byte structure.
        /// </summary>
        public const int LENGTH_VENDOR_ID = 2;
        /// <summary>
        /// The offset for the bytes which represents the command id in the byte structure.
        /// </summary>
        public const int OFFSET_COMMAND_ID = 6;
        /// <summary>
        /// The number of bytes which represents the command id in the byte structure.
        /// </summary>
        public const int LENGTH_COMMAND_ID = 2;
        /// <summary>
        /// The offset for the bytes which represents the payload in the byte structure.
        /// </summary>
        public const int OFFSET_PAYLOAD = 8;
        /// <summary>
        /// The protocol version to use for these packets.
        /// </summary>
        public const int PROTOCOL_VERSION = 1;
        /// <summary>
        /// The number of bytes for the check value.
        /// </summary>
        public const int CHECK_LENGTH = 1;
        /// <summary>
        /// The SOF - Start Of Frame - value to use for these packets.
        /// </summary>
        public const byte SOF = 0xFF;

        // To know if the checksum is in use for this packet.
        // By default there is no checksum for the packet.
        private readonly bool mHasChecksum = false;

        /// <summary>
        /// Constructor that builds a command from a byte sequence.
        /// </summary>
        /// <param name="_source">Array of bytes to build the command from.</param>
        public GaiaPacketBREDR(byte[] _source)
        {
            int flags = _source[OFFSET_FLAGS];
            int payloadLength = _source.Length - OFFSET_PAYLOAD;

            if ((flags & FLAG_CHECK_MASK) != 0)
            {
                --payloadLength;
            }

            VendorId = GaiaUtils.ExtractIntFromByteArray(_source, OFFSET_VENDOR_ID, LENGTH_VENDOR_ID, false);
            CommandId = GaiaUtils.ExtractIntFromByteArray(_source, OFFSET_COMMAND_ID, LENGTH_COMMAND_ID, false);

            if (payloadLength > 0)
            {
                Payload = new byte[payloadLength];
                Array.Copy(_source, OFFSET_PAYLOAD, Payload, 0, payloadLength);
            }

            Bytes = _source;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which composed the packet.
        /// Using this constructor, the flags will be set to false.
        /// </summary>
        /// <param name="_vendorId">The packet vendor ID.</param>
        /// <param name="_commandId">The packet command ID.</param>
        public GaiaPacketBREDR(int _vendorId, int _commandId)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = new byte[0];
            mHasChecksum = false;
            Bytes = null;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which composed the packet.
        /// </summary>
        /// <param name="_vendorId">The packet vendor ID.</param>
        /// <param name="_commandId">The packet command ID.</param>
        /// <param name="_hasChecksum">A boolean to know if a flag should be applied to this packet.</param>
        public GaiaPacketBREDR(int _vendorId, int _commandId, bool _hasChecksum)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = new byte[0];
            mHasChecksum = _hasChecksum;
            Bytes = null;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which composed the packet.
        /// Using this constructor, the flags will be set to false.
        /// </summary>
        /// <param name="_vendorId">The packet vendor ID.</param>
        /// <param name="_commandId">The packet command ID.</param>
        /// <param name="_payload">The packet payload.</param>
        public GaiaPacketBREDR(int _vendorId, int _commandId, byte[] _payload)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = _payload;
            mHasChecksum = false;
            Bytes = null;
        }

        /// <summary>
        /// Constructor that builds a packet with the information which composed the packet.
        /// </summary>
        /// <param name="_vendorId">The packet vendor ID.</param>
        /// <param name="_commandId">The packet command ID.</param>
        /// <param name="_payload">The packet payload.</param>
        /// <param name="_hasChecksum">A boolean to know if a flag should be applied to this packet.</param>
        public GaiaPacketBREDR(int _vendorId, int _commandId, byte[] _payload, bool _hasChecksum)
        {
            VendorId = _vendorId;
            CommandId = _commandId;
            Payload = _payload;
            mHasChecksum = _hasChecksum;
            Bytes = null;
        }

        /// <summary>
        /// To build the byte array which represents this Gaia Packet over BR/EDR.
        /// The bytes array is built according to the definition of a GAIA Packet sent over BR/EDR:
        /// 0 bytes  1         2        3        4        5        6        7         8         9       len+8
        /// +--------+---------+--------+--------+--------+--------+--------+--------+ +--------+--------+ +--------+
        /// |   SOF  | VERSION | FLAGS  | LENGTH |    VENDOR ID    |   COMMAND ID    | | PAYLOAD   ...   | | CHECK  |
        /// +--------+---------+--------+--------+--------+--------+--------+--------+ +--------+--------+ +--------+
        /// </summary>
        /// <param name="_commandId">The command ID for the packet bytes to build:
        /// The original command ID of this packet.
        /// The acknowledgement version of the original command ID.</param>
        /// <param name="_payload">The payload to include in the packet bytes.</param>
        /// <returns>A new byte array built with the given information.</returns>
        public override byte[] BuildBytes(int _commandId, byte[] _payload)
        {
            // if the payload is bigger than the maximum size: packet won't be sent.
            if (_payload.Length > MAX_PAYLOAD)
            {
                throw new GaiaException(GaiaException.Type.PAYLOAD_LENGTH_TOO_LONG);
            }

            int length = _payload.Length + OFFSET_PAYLOAD + (mHasChecksum ? CHECK_LENGTH : 0);
            byte[] data = new byte[length];
            data[OFFSET_SOF] = SOF;
            data[OFFSET_VERSION] = (byte)PROTOCOL_VERSION;
            data[OFFSET_FLAGS] = 0x01;
            if (!mHasChecksum)
            {
                data[OFFSET_FLAGS] = 0x00;
            }
            data[OFFSET_LENGTH] = (byte)Payload.Length;

            GaiaUtils.CopyIntIntoByteArray(VendorId, data, OFFSET_VENDOR_ID, LENGTH_VENDOR_ID, false);
            GaiaUtils.CopyIntIntoByteArray(_commandId, data, OFFSET_COMMAND_ID, LENGTH_COMMAND_ID, false);

            Array.Copy(_payload, 0, data, OFFSET_PAYLOAD, _payload.Length);

            // if there is a checksum, calculating the checksum value
            if (mHasChecksum)
            {
                byte check = 0;
                for (int i = 0; i < length - 1; i++)
                {
                    check ^= data[i];
                }
                data[length - 1] = check;
            }

            return data;
        }

        public override int GetPayloadMaxLength()
        {
            return MAX_PAYLOAD;
        }
    }
}
