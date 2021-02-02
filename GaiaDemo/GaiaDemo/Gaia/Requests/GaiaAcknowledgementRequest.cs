using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaDemo.Gaia.Requests
{
    /// <summary>
    /// The data structure to define an acknowledgement request.
    /// </summary>
    public class GaiaAcknowledgementRequest : GaiaRequest
    {
        private static Type _type;

        public GaiaAcknowledgementRequest(GAIA.Status status, byte[] data) : base(_type)
        {
            Request = Type.ACKNOWLEDGEMENT;
            Status = status;
            Data = data;
        }

        /// <summary>
        /// The status for the acknowledgement.
        /// </summary>
        public GAIA.Status Status { get; set; }

        /// <summary>
        /// Any data to add to the ACK.
        /// </summary>
        public byte[] Data { get; set; }

        
    }
}
