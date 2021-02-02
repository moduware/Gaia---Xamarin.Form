using GaiaDemo.Gaia.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaDemo.Gaia.Requests
{
    /// <summary>
    /// The data structure to define a GAIA request.
    /// </summary>
    public class GaiaRequest
    {
        /// <summary>
        /// All types of GAIA requests which can be sent to a device.
        /// </summary>
        public enum Type
        {
            SINGLE_REQUEST = 1,
            ACKNOWLEDGEMENT = 2,
            UNACKNOWLEDGED_REQUEST = 3
        }

        /// <summary>
        /// The type of the request.
        /// </summary>
        public Type Request { get; set; }

        /// <summary>
        /// If this request is about a characteristic, the Bluetooth characteristic for this request.
        /// </summary>
        public GaiaPacket Packet { get; set; }

        /// <summary>
        /// To build a new object of the type request.
        /// </summary>
        /// <param name="_type">The type of the request.</param>
        public GaiaRequest(Type _type)
        {
            Request = _type;
        }
    }
}
