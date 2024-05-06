
#pragma warning disable 8600

using System.IO;

class csprojLink
{
    private static String Path { get; set; } = String.Empty;
    private static String Link { get; set; } = String.Empty;
    private static List<string>? FileContentList { get; set; } = null;

    private static int UpdateCounter { get; set; } = 0;

    static void Main(string[] args)
    {      
        bool bErr = false;
        do
        {
            /*  The argument must consist of two items 
                    
                    1) Path to the folder where the *.csproj file is located

                    2) Path that should link the ItemGroup/Compile items
            
             */

            /* error check arguments */
            if (args.Count() != 2)
            {
                if (args.Count() == 1)
                    Console.WriteLine("\n\nMissing one needed argument!");
                else
                    Console.WriteLine("\n\nMissing all needed arguments!");
                bErr = true;

                break;
            }


            /* get path to the folder where the *.csproj file is located */
            Path = args[0];

            /* check path */
            if (!Directory.Exists(Path))
            {
                Console.WriteLine("\n\nThe path does not exist!");
                bErr = true;
                break;
            }

            /* append backslash to path if missing */
            if (!Path.EndsWith("\\"))
                Path += "\\";


            /* get path that should link the ItemGroup/Compile items */
            Link = args[1];

            /* remove any quotation marks */
            Link = Link.Replace("\"", "");

            /* append backslash to link if missing */
            if (!Link.EndsWith("\\"))
                Link += "\\";

            /* log */
            Console.WriteLine(String.Format("{0}{1, -30}{2}", "\n\n", "Path to *.csproj Folder is", Path));
            Console.WriteLine(String.Format("{0}{1, -30}{2}", "\n", "Link path is", Link));


            /* get all *.csproj files in path */
            string[] files = System.IO.Directory.GetFiles(Path, "*.csproj");

            /* but return if there is less or more than one *.csproj file in that path */
            if (files.Count() != 1)
            {
                if (files.Count() == 0)
                    Console.WriteLine("\n\nNo *.csproj file found in path!");
                if (files.Count() > 1)
                    Console.WriteLine("\n\nToo much *.csproj files found in path!");
                bErr = true;    
                break;
            }

            /* new list */
            FileContentList = new List<string>();

            /* read file */
            if (ReadFile(files[0]))
            {
                Console.WriteLine("\n\n*.csproj file READ error!");
                bErr = true;
                break;
            }

            /* edit file content */
            if (CheckAndEditFile())
            {
                Console.WriteLine("\n\n*.csproj file CHECK error!");
                bErr = true;
                break;
            }

            /* update csproj file */
            if (UpdateCsprojFile(files[0]))
            {
                Console.WriteLine("\n\n*.csproj file WRITE error!");
                bErr = true;
                break;
            }

            /* done */
            break;

        } while (true);

        String key = String.Empty; //  "Press key to exit...";

        /* log */
        if (bErr)
            Console.WriteLine(String.Format("\n\n{0} {1}", "Aborting!\n\n", key));
        else if (UpdateCounter > 0)
            Console.WriteLine(String.Format("\n\n{0} {1}", "Done!\n\n", key));
        else
            Console.WriteLine("There have been no changes in file!\n\n");

        /* we just let the function run out without extra keypress */
        //var name = Console.ReadLine();

    } // END


    /// <summary>
    /// Read all content of *.csproj file
    /// </summary>
    /// <param name="file"></param>
    /// <returns>false for OK; true for ERROR</returns>
    static bool ReadFile(string file)
    {
        int iReadCount = 0;
        String strLine = String.Empty;

        using (StreamReader f = new StreamReader(file))
        {
            while ((strLine = f.ReadLine()) is not null)
            {
                iReadCount++;
                if (FileContentList != null)
                    FileContentList.Add(strLine);
            }

            f.Close();
        }

        if (iReadCount > 0)
        {
            Console.WriteLine("\nFile >> " + file + " >> READ *OK*!\n\n");
            return false;
        }

        return true;

    } // END


    /// <summary>
    /// Checks the file with search criteria and edit matching entries
    /// </summary>
    /// <returns>false for OK; true for ERROR</returns>
    static bool CheckAndEditFile()
    {
        if (FileContentList == null)
            return true;

        for (int i = 0; i < FileContentList.Count; i++)
        {           
            if ((FileContentList[i].Contains("<Compile Include=")) ||
                (FileContentList[i].Contains("<EmbeddedResource Include=")) ||
                (FileContentList[i].Contains("<Resource Include=")) ||
                (FileContentList[i].Contains("<None Include=")))
            {
                /* get split items */
                String[] split = FileContentList[i].Split('"');

                /* skip if include has a path */
                if ((split[1].Contains("..\\")) ||
                    (split[1].Contains(":")))
                    continue;

                /* set new content */
                FileContentList[i] = split[0] + "\"" + Link + split[1] + "\"" + " Link=\"" + split[1] + "\"" + split[split.Count() -1];

                /* increment counter */
                UpdateCounter++;

                /* log */
                Console.WriteLine(FileContentList[i]);
            }
        }
        return false;

    } // END

    /// <summary>
    /// Updates the csproj file 
    /// </summary>
    /// <param name="file"></param>
    /// <returns>false for OK; true for ERROR</returns>
    static bool UpdateCsprojFile(string file)
    {
        if (UpdateCounter == 0)           
            return false;

        if (FileContentList == null)
            return true;

        using (StreamWriter f = new StreamWriter(file, false))
        {
            /* write file content */
            foreach (String thisString in FileContentList)
            {
                /* write line */
                f.WriteLine(thisString);
            }

            /* close */
            f.Close();

            /* log */
            Console.WriteLine("\n\nFile >> " + file + " >> WRITE *OK*!");

        } // end using f

        return false;

    } // END


} // class