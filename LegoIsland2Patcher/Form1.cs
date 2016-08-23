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
        bool foundExe = false;
        List<modinfo> mods = new List<modinfo>();


        public Form1()
        {
            InitializeComponent();
        }


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


        private void programStart()
        {
            if (File.Exists("LEGO Island 2.exe"))
            {
                foundExe = true;               
            }

            if (foundExe == true)
            {
                //Backup folder setup
                if (!Directory.Exists("backup"))
                {
                    Directory.CreateDirectory("backup");
                    byte[] b = new byte[0];
                    File.WriteAllBytes("backup/_Do Not Delete This Folder", b);
                }

                //Load mods
                populateModList();
            }
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


        private void installPatch(string location)
        {
            if (File.Exists(location + "/patch.txt"))
            {
                //Read patch info
                StreamReader sr = new StreamReader(location + "/patch.txt");

                string file = "LEGO Island 2.exe";

                /*
                //Check if first line is a filename or a patch line                
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
                                originals = ToBytes(parts[1]);
                            }

                            //Check if patch is valid
                            bool validpatch = false;
                            if (originals == null)
                            {
                                validpatch = true;
                            }
                            else
                            {
                                byte[] virtualFile = File.ReadAllBytes("backup/" + file);

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
                                byte[] virtualFile = File.ReadAllBytes(file);

                                int count = 0;
                                foreach (byte p in patch)
                                {
                                    virtualFile[offset + count] = p;
                                    count += 1;
                                }

                                File.WriteAllBytes(file, virtualFile);
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

                //First run: Delete installed directory
                //Second run: Delete backedup directory
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



        private void applyMods()
        {
            //Backup exe
            if (!File.Exists("backup/LEGO Island 2.exe"))
            {
                File.WriteAllBytes("backup/LEGO Island 2.exe", File.ReadAllBytes("LEGO Island 2.exe"));
            }

            //Reset
            File.WriteAllBytes("LEGO Island 2.exe", File.ReadAllBytes("backup/LEGO Island 2.exe"));

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

    }

    public class modinfo
    {
        public string name;
        public string author;
        public string description;
        public string location;
    }
}
