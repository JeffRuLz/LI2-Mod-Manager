using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace LegoIsland2Patcher
{ 

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Global values
        exeVersion exeData; //Holds names and offsets that change depending on the game version
        List<modinfo> mods = new List<modinfo>(); //List containing mod data      


        //Program start
        private void programStart()
        {
            //Firstly, figure out which exe version to use
            identifyVersion();

            if (File.Exists(exeData.exeName))
            {
                //Setup backup folder
                if (!Directory.Exists("backup"))
                {
                    Directory.CreateDirectory("backup");
                    byte[] b = new byte[0];
                    File.WriteAllBytes("backup/_Do Not Delete This Folder", b);
                }

                backupExe();

                //Load mods
                populateModList();

                //Setup check boxes
                checkResolutionMod();
                checkLoadFix();
                checkNoVideos();
            }
            else
            {
                //Disable buttons if exe file wasn't found
                btnApply.Enabled = false;
                cbLoadFix.Enabled = false;
                cbEnableResolution.Enabled = false;
                clbPatches.Enabled = false;
                cbNoVideos.Enabled = false;

                lblExeVersion.Text = "No game file found.";
            }
        }


        //Figure out which version of the game the user has
        private void identifyVersion()
        {
            //List of known versions
            exeVersion[] exeVersions = { createExeVersion("English Version 1",    0x52, "LEGO Island 2.exe", 0x529D, 7,  21, 0xA495, 0x2A870),
                                         createExeVersion("English Version 2",    0x7A, "LEGO Island 2.exe", 0x529D, 7,  21, 0xA495, 0x2A870),
                                         createExeVersion("Spanish Version",      0xCF, "Isola LEGO 2.exe",  0x52FD, 7,  21, 0xA765, 0x3DFA0),
                                         createExeVersion("Dutch Version",        0x98, "LEGO eiland 2.exe", 0x52D6, 12, 31, 0xCAC3, 0x43C70),
                                         createExeVersion("German Version",       0x31, "LEGO Insel 2.exe",  0x52FD, 7,  21, 0xA765, 0x3E1A0),
                                         createExeVersion("Swedish Version",      0x08, "LEGO Island 2.exe", 0x52D6, 12, 31, 0xCAC3, 0x43C70),
                                         createExeVersion("Norwegian Version",    0xF6, "LEGO Island 2.exe", 0x52D6, 12, 31, 0xCAC3, 0x43C70),
                                         createExeVersion("Unidentified Version", 0x52, "LEGO Island 2.exe", 0x529D, 7,  21, 0xA495, 0x2A870) };

            //If a known version was not found, default to the unidentified version
            int foundVersionIndex = exeVersions.Length - 1;
            
            //Loop through all versions
            int counter = 0;
            bool foundVersion = false;
            int firstFoundName = -1;

            foreach (exeVersion ev in exeVersions)
            {
                //Stop searching if a version was found
                if (foundVersion == false)
                {
                    if (File.Exists(ev.exeName))
                    {
                        if (firstFoundName == -1)
                        {
                            firstFoundName = counter;
                        }

                        //Check for identifyer byte
                        long byteCheckOffset = 0x128; //Offset of first byte that's different for every version

                        byte[] virtualFile = File.ReadAllBytes(ev.exeName);

                        if (virtualFile[byteCheckOffset] == ev.checkByte) {
                            foundVersionIndex = counter;
                            foundVersion = true;
                        }
                    }

                    counter++;
                }
            }

            //If no proper version was found, use the first matching filename
            if (foundVersion == false && firstFoundName != -1)
            {
                foundVersionIndex = firstFoundName;
            }
            
            //Copy found data to global data
            exeData = exeVersions[foundVersionIndex];

            //Rename label
            lblExeVersion.Text = exeData.label;
        }

        
        //Create a backup exe if one does not exist
        private void backupExe()
        {
            string backupName = "backup/" + exeData.exeName;

            if (!File.Exists(backupName)) {
                File.WriteAllBytes(backupName, File.ReadAllBytes(exeData.exeName));
            }
        }       


        private void installPatch(string location)
        {
            if (File.Exists(location + "/patch.txt"))
            {
                //Read patch info
                StreamReader sr = new StreamReader(location + "/patch.txt");
                
                if (File.Exists(exeData.exeName))
                {
                    //Read all patches
                    while (!sr.EndOfStream)
                    {
                        string raw = sr.ReadLine();
                        string[] parts = raw.Split(';');

                        //Cannot continue unless patch is in the right format
                        //todo: Display a message saying it's not correct
                        if (parts.Length >= 2)
                        {
                            long offset = HexToLong(parts[0]);
                            byte[] patch = ToBytes(parts[1]);
                            byte[] originals = null;

                            if (parts.Length >= 3)
                            {
                                originals = ToBytes(parts[2]);
                            }

                            //Check if patch is valid
                            bool validpatch = false;
                            if (originals == null)
                            {
                                validpatch = true;
                            }
                            else
                            {
                                byte[] virtualFile = File.ReadAllBytes("backup/" + exeData.exeName);

                                int count = 0;
                                bool originalmatch = true;

                                foreach (byte o in originals)
                                {
                                    if (virtualFile[offset + count] != o)
                                    {
                                        originalmatch = false;
                                        break;
                                    }
                                    count += 1;
                                }

                                validpatch = originalmatch;
                            }

                            //Apply patch
                            if (validpatch == true)
                            {
                                byte[] virtualFile = File.ReadAllBytes(exeData.exeName);

                                int count = 0;
                                foreach (byte p in patch)
                                {
                                    virtualFile[offset + count] = p;
                                    count += 1;
                                }

                                File.WriteAllBytes(exeData.exeName, virtualFile);
                            }
                        }
                    }
                }

            }            
        }

        private void uninstallPatch(string location)
        {
            
        }


        private void modFileInstall(string dir)
        {
            /*Steps
             * Check if bob/bod files are still present
             * If they are not, backup old files, but only if not already backed up
             * Install new files
             */
            
            bool backupFile = true;

            string[] parts = dir.Split('\\');

            string[] partsagain = dir.Split('/', '\\');
            if (partsagain[2] == "_data" && partsagain.Length >= 4)
            {
                string testbob = "_data/" + partsagain[3];

                if (File.Exists(testbob + ".bob") && File.Exists(testbob + ".bod"))
                {
                    backupFile = false;
                }
            }
                           
            //If directory exists in the LI2 installation
            if (Directory.Exists(dir.Split('/')[1]))
            {
                //Create directory in backup folder
                if (backupFile == true)
                {
                    if (!Directory.Exists("backup/" + dir.Split('/')[1]))
                    {
                        Directory.CreateDirectory("backup/" + dir.Split('/')[1]);
                    }
                }
            }
            else
            {
                //If still using bod/bob files, create new folder                
                Directory.CreateDirectory(dir.Split('/')[1]);
            }

            //Install files in directory
            string[] files = Directory.GetFiles(dir);
            foreach (string f in files)
            {
                parts = f.Split('/');

                if (backupFile == true)
                {
                    //Backup file if not already backed up
                    if (File.Exists(parts[1]) && !File.Exists("backup/" + parts[1]))
                    {                           
                        File.WriteAllBytes("backup/" + parts[1], File.ReadAllBytes(parts[1]));
                    }
                }
                //Install file
                File.WriteAllBytes(parts[1], File.ReadAllBytes(f));
            }

            //Search inner folders
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string s in dirs)
            {
                modFileInstall(s);
            }
        }

        private void modFileUninstall(string dir)
        {
            bool backupFile = true;

            string[] parts = dir.Split('\\');
            string[] partsagain = dir.Split('/', '\\');

            if (partsagain[2] == "_data" && partsagain.Length >= 4)
            {                
                string testbob = "_data/" + partsagain[3];

                if (File.Exists(testbob + ".bob") && File.Exists(testbob + ".bod"))
                {                    
                    backupFile = false;
                }  
            }

            string[] files = Directory.GetFiles(dir);

            foreach (string f in files)
            {
                parts = f.Split('/');

                if (backupFile == true)
                {
                    if (File.Exists("backup/" + parts[1]) )
                    {
                        File.WriteAllBytes( parts[1], File.ReadAllBytes("backup/" + parts[1]) );
                        File.Delete( "backup/" + parts[1] );
                    }
                }
                else
                {
                    //Just delete if bob/bod files exist
                    if (File.Exists(parts[1]))
                    {                        
                        File.Delete(parts[1]);
                    }                    
                }
            }


            //Search inner folders
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string s in dirs)
            {
                modFileUninstall(s);
            }
            
            //Remove empty folders
            parts = dir.Split('\\', '/');

            if (parts[0] == "mods")
            {
                string installPath = "";
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i >= 2)
                    {
                        installPath += parts[i] + "/";
                    }
                }

                //First pass: Delete installed directory
                //Second pass: Delete backedup directory
                for (int i = 0; i < 2; i++)
                {
                    if (Directory.Exists(installPath))
                    {
                        if (Directory.GetDirectories(installPath).Length == 0 && Directory.GetFiles(installPath).Length == 0)
                        {
                            Directory.Delete(installPath);
                        }
                    }
                    installPath = "backup/" + installPath;
                }
            }
           
        }



        private void installMod(string location)
        {
            if (Directory.Exists(location + "/_data"))
            {
                modFileInstall(location + "/_data");
            }
            
            if (!File.Exists(location + "/installed"))
            {
                byte[] b = new byte[0];
                File.WriteAllBytes(location + "/installed", b);
            }
        }

        private void uninstallMod(string location)
        {
            if (File.Exists(location + "/installed"))
            {
                if (Directory.Exists(location + "/_data"))
                {
                    modFileUninstall(location + "/_data");
                }

                File.Delete(location + "/installed");
            }
        }


        //Write new resolutions to file
        private void applyResolution()
        {
            //long[] resOffsetW = { exeData.resOffset, exeData.resOffset + 21, exeData.resOffset + 42, exeData.resOffset + 63 };
            //long[] resOffsetH = { exeData.resOffset + 7, exeData.resOffset + 28, exeData.resOffset + 49, exeData.resOffset + 72 };

            int resSep = exeData.resSep;
            int resDis = exeData.resDis;

            long[] resOffsetW = { exeData.resOffset,
                                  exeData.resOffset + resDis,
                                  exeData.resOffset + (resDis * 2),
                                  exeData.resOffset + (resDis * 3) };

            long[] resOffsetH = { resOffsetW[0] + resSep,
                                  resOffsetW[1] + resSep,
                                  resOffsetW[2] + resSep,
                                  resOffsetW[3] + resSep };

            bool undoHack = true;            

            if (cbEnableResolution.Checked == true)
            {
                //Check if resolution inputs are valid
                long customWidth = StringToInt(tbWidth.Text);
                long customHeight = StringToInt(tbHeight.Text);

                float customAspect = 3f / 4f;

                //Ignore if the inputs are invalid
                if (customWidth != 0 && customHeight != 0)
                {
                    undoHack = false;

                    //Custom aspect ratio
                    if (cbEnableAspect.Checked == true) {
                        customAspect = (float)customHeight / (float)customWidth;
                    }

                    //Apply patches
                    for (int i = 0; i < 4; i++) {
                        applyPatch(BitConverter.GetBytes(customWidth), resOffsetW[i], 4);
                        applyPatch(BitConverter.GetBytes(customHeight), resOffsetH[i], 4);
                    }
                    applyPatch(BitConverter.GetBytes(customAspect), exeData.fovOffset, 4);
                }
            }

            //Undo resolution hacks
            if (undoHack == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    undoPatch(resOffsetW[i], 4);
                    undoPatch(resOffsetH[i], 4);
                }
                undoPatch(exeData.fovOffset, 4);
            }
        }


        private void applyNoVideos()
        {
            //No intro videos seem to be the same across versions
            byte[] nullBytes = { 0x90, 0x90, 0x90, 0x90, 0x90 };
            long[] offsets = { 0x2AEA, 0x2B20, 0x2B56 };

            if (cbNoVideos.Checked == true)
            {
                foreach (long off in offsets)
                {
                    applyPatch(nullBytes, off, nullBytes.Length);
                }
            }
            else
            {
                foreach (long off in offsets)
                {
                    undoPatch(off, nullBytes.Length);
                }
            }
        }

        private void applyPatch(byte[] bytes, long offset, int length)
        {
            byte[] virtualReal = File.ReadAllBytes(exeData.exeName);

            for (int i = 0; i < length; i++)
            {
                byte thisbyte = 0;
                if (i <= bytes.Length)
                {
                    thisbyte = bytes[i];
                }

                virtualReal[offset + i] = thisbyte;
            }

            File.WriteAllBytes(exeData.exeName, virtualReal);
        }

        //Returns the original bytes from the backup file into the real one
        private void undoPatch(long offset, int length)
        {
            String backuppath = "backup/" + exeData.exeName;

            if (File.Exists(backuppath))
            {
                byte[] virtualBackup = File.ReadAllBytes(backuppath);
                byte[] virtualReal = File.ReadAllBytes(exeData.exeName);

                for (int i = 0; i < length; i++)
                {
                    virtualReal[offset + i] = virtualBackup[offset + i];
                }

                File.WriteAllBytes(exeData.exeName, virtualReal);
            }
        }


        private void applyLoadFix()
        {
            byte[] nullBytes = { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 };

            if (cbLoadFix.Checked == true)
            {
                applyPatch(nullBytes, exeData.loadOffset, nullBytes.Length);
            }
            else
            {
                undoPatch(exeData.loadOffset, nullBytes.Length);
            }
        }

        private void applyMods()
        {
            string exeName = exeData.exeName;
            string backupName = "backup/" + exeName;

            backupExe();

            //Reset
            File.WriteAllBytes(exeName, File.ReadAllBytes(backupName));

            //Apply Custom Resolution
            applyResolution();

            //Load Time Fix
            applyLoadFix();

            //No intro videos
            applyNoVideos();

            //Loop through check boxes
            for (int i = 0; i < clbPatches.Items.Count; i++)
            {
                if (clbPatches.GetItemChecked(i) == true)
                {
                    installPatch(mods[i].location);
                    installMod(mods[i].location);
                }
                else
                {
                    uninstallPatch(mods[i].location);
                    uninstallMod(mods[i].location);
                }
            }

            MessageBox.Show("Done");
        }

        static private exeVersion createExeVersion(string label, byte checkByte, string exeName, long resOff, int resSep, int resDis, long fovOff, long loadOff)
        {
            exeVersion result = new exeVersion();

            result.label = label;
            result.checkByte = checkByte;
            result.exeName = exeName;
            result.resOffset = resOff;
            result.resSep = resSep;
            result.resDis = resDis;
            result.fovOffset = fovOff;
            result.loadOffset = loadOff;

            return result;
        }


        //Check if a series of bytes exist in a file
        private bool bytesMatch(string filename, long offset, byte[] bytes)
        {
            bool match = true;
            byte[] virtualFile = File.ReadAllBytes(filename);

            for (int i = 0; i < bytes.Length; i++)
            {
                if (virtualFile[offset + i] != bytes[i])
                {
                    match = false;
                }
            }

            return match;
        }


//----------Functions that fill in inputs---------------
        private void populateModList()
        {
            //Read mods folder
            if (Directory.Exists("mods"))
            {
                string[] folders = Directory.GetDirectories("mods");

                foreach (string f in folders)
                {
                    //Create Mod
                    modinfo mi = new modinfo();
                    mi.location = f;
                    mi.name = f.Split('/', '\\')[f.Split('/', '\\').Length - 1]; //Default name is folder's name
                    mi.author = "Unknown";
                    mi.description = "No description available.";

                    //Find info.txt
                    if (File.Exists(f + "/info.txt"))
                    {
                        StreamReader reader = new StreamReader(f + "/info.txt");

                        mi.name = reader.ReadLine();
                        mi.author = reader.ReadLine();

                        if (!reader.EndOfStream)
                        {
                            mi.description = "";
                        }

                        while (!reader.EndOfStream)
                        {
                            mi.description += reader.ReadLine() + "\n";
                        }
                    }

                    //Add check box
                    mods.Add(mi);
                    clbPatches.Items.Add(mi.name + " - " + mi.author);

                    //if installed file exists, check box
                    bool check = false;

                    if (File.Exists(f + "/installed"))
                    {
                        check = true;
                    }
                    else
                    {
                        //Check if file is already patched
                        if (File.Exists(f + "/patch.txt"))
                        {
                            StreamReader sr = new StreamReader(f + "/patch.txt");

                            //Read file to patch
                            string file = "LEGO Island 2.exe"; //Default file to patch

                            /*
                            //Detect if first line is actually a patch line
                            string tempfile = sr.ReadLine();
                            if (tempfile.Split(';').Length <= 1) {
                                file = tempfile;
                            }
                            else
                            {
                                sr.BaseStream.Position = 0;
                                sr.DiscardBufferedData();
                            }
                            */

                            if (File.Exists(file))
                            {
                                byte[] virtualFile = File.ReadAllBytes(file);

                                while (!sr.EndOfStream && check == false)
                                {
                                    string raw = sr.ReadLine();

                                    //Don't bother checking if not in the right format. Prevents errors too.
                                    if (raw.Split(';').Length >= 2)
                                    {
                                        long offset = HexToLong(raw.Split(';')[0]);
                                        byte[] patch = ToBytes(raw.Split(';')[1]);

                                        int count = 0;
                                        bool patchmatch = true;

                                        check = true;
                                        foreach (byte b in patch)
                                        {
                                            if (virtualFile[offset + count] != b)
                                            {
                                                patchmatch = false;
                                                break;
                                            }
                                            count += 1;
                                        }

                                        check = patchmatch;
                                    }
                                }
                            }
                            else
                            {
                                check = false;
                            }
                        }
                    }

                    clbPatches.SetItemChecked(clbPatches.Items.Count - 1, check);
                }
            }
        }


        //Check load fix box
        private void checkLoadFix()
        {
            byte[] nullBytes = { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90 };

            if (bytesMatch(exeData.exeName, exeData.loadOffset, nullBytes) == true)
            {
                cbLoadFix.Checked = true;
            }
        }


        //Fill in resolution input
        private void checkResolutionMod()
        {
            int resSep = exeData.resSep;
            int resDis = exeData.resDis;

            long[] resOffsetW = { exeData.resOffset,
                                  exeData.resOffset + resDis,
                                  exeData.resOffset + (resDis * 2),
                                  exeData.resOffset + (resDis * 3) };

            long[] resOffsetH = { resOffsetW[0] + resSep,
                                  resOffsetW[1] + resSep,
                                  resOffsetW[2] + resSep,
                                  resOffsetW[3] + resSep };
           /*
            long[] resOffsetH = { exeData.resOffset + resSep,
                                  exeData.resOffset + resSep + resDis,
                                  exeData.resOffset + resSep + (resDis * 2),
                                  exeData.resOffset + resSep + (resDis };
            * */

            byte[] virtualReal = File.ReadAllBytes(exeData.exeName);
            byte[] virtualBackup = File.ReadAllBytes("backup/" + exeData.exeName);

            bool difFOV = false;
            bool difRes = false;

            //Check for resolution differences
            for (int a = 0; a < 4; a++)
            {
                int off = (int)resOffsetW[0] + a;
                if (virtualReal[off] != virtualBackup[off])
                {
                    difRes = true;
                }

                off = (int)exeData.fovOffset + a;
                if (virtualReal[off] != virtualBackup[off])
                {
                    difFOV = true;
                }
            }

            if (difRes == true)
            {
                cbEnableResolution.Checked = true;
                byte[] wbytes = { virtualReal[resOffsetW[0]], virtualReal[resOffsetW[0] + 1], virtualReal[resOffsetW[0] + 2], virtualReal[resOffsetW[0] + 3] };
                byte[] hbytes = { virtualReal[resOffsetH[0]], virtualReal[resOffsetH[0] + 1], virtualReal[resOffsetH[0] + 2], virtualReal[resOffsetH[0] + 3] };

                tbWidth.Text = BitConverter.ToInt32(wbytes, 0) + "";
                tbHeight.Text = BitConverter.ToInt32(hbytes, 0) + "";
            }

            if (difFOV == true)
            {
                cbEnableAspect.Checked = true;
            }
        }


        private void checkNoVideos()
        {
            //No intro videos seem to be the same across versions
            byte[] nullBytes = { 0x90, 0x90, 0x90, 0x90, 0x90};
            long[] offsets   = { 0x2AEA, 0x2B20, 0x2B56 };

            bool patchApplied = true;

            foreach (long off in offsets)
            {
                if (bytesMatch(exeData.exeName, off, nullBytes) == false)
                {
                    patchApplied = false;
                }
            }

            cbNoVideos.Checked = patchApplied;
        }

//-----------Conversion functions------------
        //Returns 0 if the string is not a number or less than zero
        private int StringToInt(String s)
        {
            if (s.All(char.IsDigit) == true)
            {
                int result = Int32.Parse(s);

                if (result > 0)
                {
                    return result;
                }
            }

            return 0;
        }


        private long HexToLong(string hex)
        {
            hex = hex.Trim();

            long result = 0;
            char[] hexchars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            int position = 0;
            for (int i = hex.Length - 1; i >= 0; i--)
            {
                int convert = 0;

                for (int place = 0; place < 16; place++)
                {
                    if (Char.ToLower(hex[i]) == hexchars[place])
                    {
                        convert = place;
                        place = 16;
                    }
                }

                result += convert * (int)Math.Pow(16, position);

                position += 1;
            }

            return result;
        }


        private byte[] ToBytes(string raw)
        {
            raw = raw.Trim();

            if (raw == "")
            {
                return null;
            }
            else
            {
                if (raw.Length % 2 != 0)
                {
                    raw = '0' + raw;
                }

                int numofbytes = raw.Length / 2;
                byte[] result = new byte[numofbytes];

                for (int i = 0; i < raw.Length; i += 2)
                {
                    result[i / 2] = (byte)HexToLong(Char.ToString(raw[i]) + Char.ToString(raw[i + 1]));
                }

                return result;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            programStart();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            applyMods();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.rockraidersunited.com/");
            Process.Start(sInfo);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.github.com/JeffRuLz/LI2-Mod-Manager");
            Process.Start(sInfo);
        }

        private void clbPatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtbDescription.Text = mods[clbPatches.SelectedIndex].description;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cbResolutions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbEnableResolution_CheckedChanged(object sender, EventArgs e)
        {
            gbResolution.Enabled = cbEnableResolution.Checked;
        }

    }

    public class modinfo
    {
        public string name;
        public string author;
        public string description;
        public string location;
    }

    public class exeVersion
    {
        public string label;
        public byte checkByte; //Byte value at position 0x128
        public string exeName;
        public long resOffset; //See https://www.rockraidersunited.com/topic/7653-widescreen-hack-high-resolution/?tab=comments#comment-129128
        public int resSep; //Distance from W to H
        public int resDis; //Distance from one resolution to the next
        public long fovOffset; //Search for 00 00 40 3F
        public long loadOffset; //Long loading fix. Search for 90 90 90 90 90 FF. Use the position of FF.
    }
}
