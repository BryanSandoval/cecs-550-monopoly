﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Networking
{
    public class Packet
    {
        public enum PACKET_FLAG
        {
            SYSTEM_READ = 0,
            USER_READ = 1
        }

        private PACKET_FLAG _DestinationFlag;
        private static int GUID_LENGTH = 16;
        private Guid _SenderGuid;
        private byte[] _Message;

        public PACKET_FLAG DestinationFlag
        {
            get { return _DestinationFlag; }
        }

        public Guid SenderGuid
        {
            get { return _SenderGuid; }
        }

        public byte[] Message
        {
            get { return _Message; }
        }

        public Packet(PACKET_FLAG DestinationFlag, Guid SenderGuid, byte[] Message)
        {
            this._DestinationFlag = DestinationFlag;
            this._SenderGuid = SenderGuid;
            this._Message = Message;
        }

        public Packet(byte[] ByteArray)
        {
            this._DestinationFlag = ByteArray[0] == 0 ? PACKET_FLAG.SYSTEM_READ : PACKET_FLAG.USER_READ;
            byte[] RemoteGuidArray = new byte[GUID_LENGTH];
            Buffer.BlockCopy(ByteArray, 1, RemoteGuidArray, 0, GUID_LENGTH);
            this._SenderGuid = new Guid(RemoteGuidArray);
            this._Message = new byte[ByteArray.Length - GUID_LENGTH - 1];
            Buffer.BlockCopy(ByteArray, 1 + GUID_LENGTH, _Message, 0, _Message.Length);
        }

        public byte[] ToBytes()
        {
            byte[] bytePacket = new byte[_SenderGuid.ToByteArray().Length + _Message.Length + 1];
            Buffer.BlockCopy(new byte[]{(byte) _DestinationFlag}, 0, bytePacket, 0, 1);
            Buffer.BlockCopy(_SenderGuid.ToByteArray(), 0, bytePacket, 1, _SenderGuid.ToByteArray().Length);
            Buffer.BlockCopy(_Message, 0, bytePacket, _SenderGuid.ToByteArray().Length + 1, _Message.Length);
            return bytePacket;
        }
    }
}