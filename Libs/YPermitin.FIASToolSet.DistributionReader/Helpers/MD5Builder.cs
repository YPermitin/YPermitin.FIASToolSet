using System.Security.Cryptography;
using System.Text;

namespace YPermitin.FIASToolSet.DistributionReader.Helpers;

public class MD5Builder : IDisposable
{
    private readonly MD5 _md5;

    public MD5Builder()
    {
        _md5 = MD5.Create();
    }

    public MD5Builder Add(int value)
    {
        AddIntToHash(_md5, value);
        return this;
    }
    
    public MD5Builder Add(long value)
    {
        AddLongToHash(_md5, value);
        return this;
    }
    
    public MD5Builder Add(DateTime value)
    {
        AddDateTimeToHash(_md5, value);
        return this;
    }
    
    public MD5Builder Add(string value)
    {
        AddStringToHash(_md5, value);
        return this;
    }
    
    public MD5Builder Add(Guid value)
    {
        AddGuidToHash(_md5, value);
        return this;
    }
    
    public MD5Builder Add(bool value)
    {
        AddBoolToHash(_md5, value);
        return this;
    }
    
    public byte[] Build()
    {
        _md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        return _md5.Hash;
    }
    
    public string BuildAsString()
    {
        var hash = Build();
        return ConvertByteArrayToString(hash);
    }

    public void Dispose()
    {
        _md5?.Dispose();
    }

    #region Service

    private void AddStringToHash(ICryptoTransform cryptoTransform, string textToHash)
    {
        var inputBuffer = Encoding.UTF8.GetBytes(textToHash);
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private void AddIntToHash(ICryptoTransform cryptoTransform, int intToHash)
    {
        var inputBuffer = BitConverter.GetBytes(intToHash);
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private void AddLongToHash(ICryptoTransform cryptoTransform, long longToHash)
    {
        var inputBuffer = BitConverter.GetBytes(longToHash);
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private void AddDateTimeToHash(ICryptoTransform cryptoTransform, DateTime dateTimeToHash)
    {
        var inputBuffer = BitConverter.GetBytes(dateTimeToHash.ToBinary());
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private void AddGuidToHash(ICryptoTransform cryptoTransform, Guid guidToHash)
    {
        var inputBuffer = guidToHash.ToByteArray();
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private void AddBoolToHash(ICryptoTransform cryptoTransform, bool boolToHash)
    {
        var inputBuffer = BitConverter.GetBytes(boolToHash);
        cryptoTransform.TransformBlock(inputBuffer, 0, inputBuffer.Length, inputBuffer, 0);
    }
    
    private string ConvertByteArrayToString(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }

    #endregion
}