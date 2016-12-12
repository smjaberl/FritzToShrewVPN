using System;
using System.IO;
using System.Linq;


public class shrewData
{
	public shrewData()
	{
        DirectoryInfo ParentDirectory = new System.IO.DirectoryInfo(@"%LOCALAPPDATA%\Shrew Soft VPN\sites";

        foreach (System.IO.FileInfo f in ParentDirectory.GetFiles())
        {
            VPNs.Add(f.Name);
        }

    }

    public List<String> VPNs = new List<string>();

    private void nix()
    {
        
    }
}
