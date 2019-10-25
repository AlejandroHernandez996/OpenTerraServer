using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class ByteBuffer : IDisposable
    {

        private List<byte> buffer;
        private byte[] readBuffer;
        private int readBufferIndex;
        private bool isBuffUpdated = false;

        public ByteBuffer()
        {
            buffer = new List<byte>();
            readBufferIndex = 0;
        }
        public int GetReadBufferIndex()
        {
            return readBufferIndex;
        }
        public int GetBufferSize()
        {
            return buffer.Count;
        }
        public int GetRemainingBufferLength()
        {
            return GetBufferSize() - GetReadBufferIndex();
        }
        public byte[] ToArray()
        {
            return buffer.ToArray();
        }
        public void ClearBuffer()
        {
            buffer.Clear();
            readBufferIndex = 0;

        }
        public void WriteByte(byte b)
        {
            buffer.Add(b);
            isBuffUpdated = true;
        }
        public void WriteBytes(byte[] bs)
        {
            foreach(byte b in bs)
            {
                buffer.Add(b);
            }
            isBuffUpdated = true;

        }
        public void WriteShort(short s)
        {
            WriteBytes(BitConverter.GetBytes(s));
        }
        public void WriteInt(int i)
        {
            WriteBytes(BitConverter.GetBytes(i));
        }
        public void WriteLong(long l)
        {
            WriteBytes(BitConverter.GetBytes(l));
        }
        public void WriteFloat(float f)
        {
            WriteBytes(BitConverter.GetBytes(f));
        }
        public void WriteBool(bool b)
        {
            WriteBytes(BitConverter.GetBytes(b));
        }
        public void WriteString(String s)
        {
            WriteBytes(BitConverter.GetBytes(s.Length));
            WriteBytes(Encoding.ASCII.GetBytes(s));
        }
        public byte[] ReadBytes(int length, bool peek = true)
        {
            if (buffer.Count > readBufferIndex)
            {
                if (isBuffUpdated)
                {
                    readBuffer = buffer.ToArray();
                    isBuffUpdated = false;
                }

                byte[] value = buffer.GetRange(readBufferIndex, length).ToArray();

                readBufferIndex = peek ? readBufferIndex + length : readBufferIndex;

                return value;
            }
            else
            {
                throw new Exception("Cannot read BYTES");
            }
        }
        public short ReadShort(bool peek = true)
        {
            return BitConverter.ToInt16(ReadBytes(2,peek),0);
        }
        public int ReadInt(bool peek = true)
        {
            return BitConverter.ToInt32(ReadBytes(4,peek), 0);
        }
        public long ReadLong(bool peek = true)
        {
            return BitConverter.ToInt16(ReadBytes(8,peek), 0);
        }
        public float ReadFloat(bool peek = true)
        {
            return BitConverter.ToSingle(ReadBytes(4,peek), 0);
        }
        public bool ReadBool(bool peek = true)
        {
            return BitConverter.ToBoolean(ReadBytes(1,peek), 0);
        }
        public string ReadString(bool peek = true)
        {
            int length = ReadInt();
            return Encoding.ASCII.GetString(ReadBytes(length,peek));
        }
        public void Dispose()
        {
            ClearBuffer();
        }
    }
}
