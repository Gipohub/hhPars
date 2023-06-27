using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using static WpfApp1Tech.MainWindow;

namespace WpfApp1Tech
{
    internal class Interpreter
    {
        public Vector<TechDictionary>? VecTech { get; set; } //обьявление словаря технологий
        public Vector<TechDictionary>? VecNoTech { get; set; } //обьявление словаря прочих слов
        public Interpreter()
        {
            
            
            
            
        }

        internal void DicReview(string wayToDictionaryPath)
        {
            try
            {


                //string date = MainWindow.DateOfList[0];
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                Vector<TechDictionary>? vec = new(); //обьявление словаря
                Settings settings = new();
                string dicPathWay = Path.Combine(settings.ParsFolder, "TechDictionary");
                try
                {


                    using var sr = new StreamReader(settings.ParsFolder);//чтение потока из указанного файла
                    using var jr = new JsonTextReader(sr);// валидауция например
                    vec = serializer.Deserialize<Vector<TechDictionary>>(jr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No acception Dictionary on this path way\n\r", ex.Message);
                    //vec = ssDef;
                }
                if (vec.Count != 0)
                {
                    Vector<TechDictionary>? vecTech; //обьявление словаря технологий
                    Vector<TechDictionary>? vecNoTech; //обьявление словаря прочих слов
                    TechDictionary dicTech = vec.Front;
                    //TechDictionary dicNoTech;

                    int maxUseWordTechCount = 0;
                    int maxUseWordNoTechCount = 0;


                    for (int i = 0; i < vec.Count; i++)
                    {

                        //for (int j = 0; j < vec.At(i).VectorPerDate.Count; j++)
                        {
                          //  if (vec.At(i).VectorPerDate.At(j).Date == wayToDictionaryPath)
                            {
                                if (vec.At(i).IsTech)
                                {
                               //     if (vec.At(i).VectorPerDate.At(j).UsingTimes > maxUseWordTechCount)
                                    {
                               //         maxUseWordTechCount = vec.At(i).VectorPerDate.At(j).UsingTimes;
                                    }
                                }
                                else
                                {
                               //     if (vec.At(i).VectorPerDate.At(j).UsingTimes > maxUseWordNoTechCount)
                                    {
                              //          maxUseWordTechCount = vec.At(i).VectorPerDate.At(j).UsingTimes;
                                    }
                                }
                            }
                        }


                    }
                    vecTech = new(maxUseWordTechCount);
                    vecNoTech = new(maxUseWordNoTechCount);

                    for (int i = 0; i < vec.Count; i++)
                    {

                      //  for (int j = 0; j < vec.At(i).VectorPerDate.Count; j++)
                        {
                       //     if (vec.At(i).VectorPerDate.At(j).Date == wayToDictionaryPath)
                            {
                                if (vec.At(i).IsTech)
                                {
                         //           vecTech[vec.At(i).VectorPerDate.At(j).UsingTimes] = vec.At(i);
                                }
                                else
                                {
                           //         vecNoTech[vec.At(i).VectorPerDate.At(j).UsingTimes] = vec.At(i);
                                }
                            }
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
