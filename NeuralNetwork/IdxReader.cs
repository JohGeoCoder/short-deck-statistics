﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkRunner
{
    public class IdxReader
    {

        public static void ReadFile()
        {
            try
            {
                Console.WriteLine("\nBegin\n");
                FileStream ifsLabels = new($"{Environment.CurrentDirectory}/../../../Data/t10k-labels.idx1-ubyte", FileMode.Open); // test labels
                FileStream ifsImages = new($"{Environment.CurrentDirectory}/../../../Data/t10k-images.idx3-ubyte", FileMode.Open); // test images

                BinaryReader brLabels = new(ifsLabels);
                BinaryReader brImages = new(ifsImages);

                int magic1 = brImages.ReadInt32(); // discard
                int numImages = brImages.ReadInt32();
                int numRows = brImages.ReadInt32();
                int numCols = brImages.ReadInt32();

                int magic2 = brLabels.ReadInt32();
                int numLabels = brLabels.ReadInt32();

                byte[][] pixels = new byte[28][];
                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i] = new byte[28];

                // each test image
                for (int di = 0; di < 10000; ++di)
                {
                    for (int i = 0; i < 28; ++i)
                    {
                        for (int j = 0; j < 28; ++j)
                        {
                            byte b = brImages.ReadByte();
                            pixels[i][j] = b;
                        }
                    }

                    byte lbl = brLabels.ReadByte();

                    DigitImage dImage = new(pixels, lbl);
                    Console.WriteLine(dImage.ToString());
                    Console.ReadLine();
                } // each image

                ifsImages.Close();
                brImages.Close();
                ifsLabels.Close();
                brLabels.Close();

                Console.WriteLine("\nEnd\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static IEnumerable<Tuple<byte[], byte>> GetInputs()
        {
            FileStream ifsLabels = new($"{Environment.CurrentDirectory}/../../../Data/t10k-labels.idx1-ubyte", FileMode.Open); // test labels
            FileStream ifsImages = new($"{Environment.CurrentDirectory}/../../../Data/t10k-images.idx3-ubyte", FileMode.Open); // test images

            BinaryReader brLabels = new(ifsLabels);
            BinaryReader brImages = new(ifsImages);

            int magic1 = brImages.ReadInt32(); // discard
            int numImages = brImages.ReadInt32();
            int numRows = brImages.ReadInt32();
            int numCols = brImages.ReadInt32();

            int magic2 = brLabels.ReadInt32();
            int numLabels = brLabels.ReadInt32();

            for (int j = 0; j < 10000; j++)
            {
                var pixels = new byte[28 * 28];

                for (int i = 0; i < 28 * 28; i++)
                {
                    pixels[i] = brImages.ReadByte();
                }

                byte lbl = brLabels.ReadByte();

                yield return Tuple.Create(pixels, lbl);
            }
        }
    }

    public class DigitImage
    {
        public byte[][] pixels;
        public byte label;

        public DigitImage(byte[][] pixels, byte label)
        {
            this.pixels = new byte[28][];
            for (int i = 0; i < this.pixels.Length; ++i)
                this.pixels[i] = new byte[28];

            for (int i = 0; i < 28; ++i)
                for (int j = 0; j < 28; ++j)
                    this.pixels[i][j] = pixels[i][j];

            this.label = label;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if (this.pixels[i][j] == 0)
                        s += " "; // white
                    else if (this.pixels[i][j] == 255)
                        s += "O"; // black
                    else
                        s += "."; // gray
                }
                s += "\n";
            }
            s += this.label.ToString();
            return s;
        } // ToString

    }
}
