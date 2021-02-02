using System;

namespace GaiaDemo.Gaia.Packet
{
    /// <summary>
    /// This class encapsulates information for a GAIA packet.
    /// </summary>
    public abstract class GaiaPacket
    {
        /// <summary>
        /// The vendor ID of the packet.
        /// </summary>
        protected int VendorId = GAIA.VENDOR_QUALCOMM;
        /// <summary>
        /// This attribute contains the full command of the packet. 
        /// If this packet is an acknowledgement packet, this attribute will contain the acknowledgement bit set to 1.
        /// </summary>
        protected int CommandId;
        /// <summary>
        /// The payload which contains all values for the specified command.
        /// If the packet is an acknowledgement packet, the first byte of the packet corresponds to the status of the sent command.
        /// </summary>
        protected byte[] Payload = new byte[0];
        /// <summary>
        /// The bytes which represent this packet.
        /// </summary>
        protected byte[] Bytes;

        /// <summary>
        /// Gets the vendor identifier for this command.
        /// </summary>
        /// <returns>The vendor identifier.</returns>
        public int GetVendorId()
        {
            return VendorId;
        }

        /// <summary>
        /// Gets the command ID including the ACK bit.
        /// </summary>
        /// <returns>The command ID.</returns>
        public int GetCommandId()
        {
            return CommandId;
        }

        /// <summary>
        /// Gets the entire payload.
        /// </summary>
        /// <returns>Array of bytes containing the payload.</returns>
        public byte[] GetPayload()
        {
            return Payload;
        }

        /// <summary>
        /// To get the bytes which correspond to this packet.
        /// </summary>
        /// <returns>A new byte array if this packet has been created using its characteristics or the source bytes if this packet has been created from a source byte array.</returns>
        public byte[] GetBytes()
        {
            if (Bytes == null)
            {
                try
                {
                    Bytes = BuildBytes(CommandId, Payload);
                }
                catch (GaiaException)
                {
                    throw new GaiaException(GaiaException.Type.PAYLOAD_LENGTH_TOO_LONG);
                }                
            }            

            return Bytes;
        }

        /// <summary>
        /// A packet is an acknowledgement packet if its command contains the acknowledgement mask.
        /// </summary>
        /// <returns>true if the command is an acknowledgement.</returns>
        public bool IsAcknowledgement()
        {
            return (CommandId & GAIA.ACKNOWLEDGMENT_MASK) > 0;
        }

        /// <summary>
        /// Gets the raw command ID for this command with the ACK bit stripped out.
        /// </summary>
        /// <returns>The command ID without the acknowledgment.</returns>
        public int GetCommand()
        {
            return CommandId & GAIA.COMMAND_MASK;
        }

        /// <summary>
        /// Gets the status byte from the payload of an acknowledgement packet.
        /// By convention in acknowledgement packets the first byte contains the command status or 'result' of the command.
        /// Additional data may be present in the acknowledgement packet, as defined by individual commands.
        /// </summary>
        /// <returns>The status code as defined in GAIA.Status.</returns>
        public GAIA.Status GetStatus()
        {
            int STATUS_OFFSET = 0;
            int STATUS_LENGTH = 1;

            if (!IsAcknowledgement() || Payload == null || Payload.Length < STATUS_LENGTH)
            {
                return GAIA.Status.NOT_STATUS;
            }
            else
            {
                return GAIA.GetStatus(Payload[STATUS_OFFSET]);
            }
        }

        /// <summary>
        /// Gets the event found in byte zero of the payload if the packet is a notification event packet.
        /// </summary>
        /// <returns>The event code according to GAIA.NotificationEvents</returns>
        public GAIA.NotificationEvents GetNotificationEvent()
        {
            int EVENT_OFFSET = 0;
            int EVENT_LENGTH = 1;

            if ((CommandId & GAIA.COMMANDS_NOTIFICATION_MASK) < 1 || Payload == null || Payload.Length < EVENT_LENGTH)
            {
                return GAIA.NotificationEvents.NOT_NOTIFICATION;
            }
            else
            {
                return GAIA.GetNotificationEvent(Payload[EVENT_OFFSET]);
            }
        }

        /// <summary>
        /// To get the acknowledgement packet bytes which correspond to this packet. Only works if the packet is not already an acknowledgement packet.
        /// </summary>
        /// <param name="_status">The status for the acknowledgement packet.</param>
        /// <param name="_value">The parameters to specify for the acknowledgement packet.</param>
        /// <returns>A new byte array created with the acknowledgement command which corresponds to this packet command.</returns>
        public byte[] GetAcknowledgementPacketBytes(int _status, byte[] _value)
        {
            if (IsAcknowledgement())
            {
                throw new GaiaException(GaiaException.Type.PACKET_IS_ALREADY_AN_ACKNOWLEDGEMENT);
            }

            int STATUS_OFFSET = 0;
            int STATUS_LENGTH = 1;
            int DATA_OFFSET = 1;
            int _commandId = CommandId | GAIA.ACKNOWLEDGMENT_MASK;
            byte[] _payload;

            if (_value != null)
            {
                int length = STATUS_LENGTH + _value.Length;
                try
                {
                    _payload = new byte[length];
                    Array.Copy(_value, 0, _payload, DATA_OFFSET, length - STATUS_LENGTH);
                }
                catch (GaiaException)
                {
                    throw new GaiaException(GaiaException.Type.PAYLOAD_LENGTH_TOO_LONG);
                }                
            }
            else
            {
                _payload = new byte[STATUS_LENGTH];
            }
            _payload[STATUS_OFFSET] = (byte)_status;            

            return BuildBytes(_commandId, _payload);
        }

        /// <summary>
        /// To build a Notification packet.
        /// The packet is built according to the definition of a GAIA Notification Packet. 
        /// The first byte of the payload is the notification event, see GAIA.NotificationEvents.
        /// The next bytes represent the data for the given event. The structure of the payload is as follows:
        ///   0 bytes  1                len
        ///   +--------+--------+--------+
        ///...| EVENT  | DATA      ...   | ...
        ///   +--------+--------+--------+
        /// </summary>
        /// <param name="_vendorID">the vendor ID of the packet.</param>
        /// <param name="_commandID">the notification command ID (0x4...)</param>
        /// <param name="_events">The notification event</param>
        /// <param name="_data">Any additional data to complete with the notification event.</param>
        /// <param name="_type">the built object of the class GaiaPacketBLE</param>
        /// <returns></returns>
        public static GaiaPacket BuildNotificationPacket(int _vendorID, int _commandID, GAIA.NotificationEvents _events, byte[] _data, GAIA.Transport _type)
        {
            if ((_commandID & GAIA.COMMANDS_NOTIFICATION_MASK) != GAIA.COMMANDS_NOTIFICATION_MASK)
            {
                throw new GaiaException(GaiaException.Type.PACKET_NOT_A_NOTIFICATION);
            }

            int EVENT_OFFSET = 0;
            int EVENT_LENGTH = 1;
            int DATA_OFFSET = 1; // EVENT_OFFSET + EVENT_LENGTH
            byte[] _payload;

            if (_data != null)
            {
                _payload = new byte[EVENT_LENGTH + _data.Length];
                _payload[EVENT_OFFSET] = (byte)_events;
                Array.Copy(_data, 0, _payload, DATA_OFFSET, _data.Length);
            }
            else
            {
                _payload = new byte[EVENT_LENGTH];
                _payload[EVENT_OFFSET] = (byte)_events;
            }

            if (_type == GAIA.Transport.BLE)
            {
                return new GaiaPacketBLE(_vendorID, _commandID, _payload);
            }
            else
            {
                return new GaiaPacketBREDR(_vendorID, _commandID, _payload);
            }
        }

        /// <summary>
        /// To build the byte array which represents this Gaia Packet.
        /// </summary>
        /// <param name="_commandId">The command ID for the packet bytes to build: 
        /// The original command ID of this packet.
        /// The acknowledgement version of the original command ID.
        /// </param>
        /// <param name="_payload">The payload to include in the packet bytes.</param>
        /// <returns>A new byte array built with the given information.</returns>
        public abstract byte[] BuildBytes(int _commandId, byte[] _payload);

        /// <summary>
        /// Depending on the type of transport used the packet format is different as well as the maximum number of bytes they can use. 
        /// This method gives the maximum number of bytes the payload can have. 
        /// The payload is a common field to any GAIA packet as well as the vendor ID and the command ID.
        /// 
        /// @deprecated since 3.0.7 The maximum length is implementation dependant.
        /// </summary>
        /// <returns>the maximum length of a payload.</returns>
        public abstract int GetPayloadMaxLength();
    }
}
