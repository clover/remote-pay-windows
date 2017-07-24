// Copyright (C) 2016 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.clover.remotepay.transport
{
    public class CaptureLog
    {
        FileStream ostrm;
        StreamWriter writer;
        TextWriter oldOut = Console.Out;

        public CaptureLog()
        {
            try
            {
                ostrm = new FileStream(Path.GetTempPath() + "Clover.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                writer.AutoFlush = true;
                Console.SetOut(writer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
            }
        }

        ~CaptureLog()
        {
            if (writer != null && ostrm != null)
            {
                Console.SetOut(writer);
                Console.WriteLine("This is a line of text");
                Console.WriteLine("Everything written to Console.Write() or");
                Console.WriteLine("Console.WriteLine() will be written to a file");
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
                Console.WriteLine("Done");
            }
        }
    }
}
