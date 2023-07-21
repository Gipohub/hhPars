using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using static WpfApp1Tech.MainWindow;

namespace WpfApp1Tech.Interpritator
{
    internal class Interpreter
    {
        public List<TechDictionary>? VecTech = new();// { get; set; } //обьявление словаря технологий
        public List<TechDictionary>? VecNoTech = new();// { get; set; } //обьявление словаря прочих слов
        public Interpreter(string wayToDictionaryPath)
        {

            DicReview(wayToDictionaryPath);



        }

        internal void DicReview(string wayToDictionaryPath)
        {
            try
            {



                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                List<TechDictionary>? vec = new(); //обьявление словаря

                try
                {


                    using var sr = new StreamReader(wayToDictionaryPath);//чтение потока из указанного файла
                    using var jr = new JsonTextReader(sr);// валидауция например
                    vec = serializer.Deserialize<List<TechDictionary>>(jr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No acception Dictionary on this path way\n\r", ex.Message);
                    //vec = ssDef;
                }
                if (vec.Count != 0)
                {
                    //List<TechDictionary>? vecTech = new(); //обьявление словаря технологий
                    //List<TechDictionary>? vecNoTech = new(); //обьявление словаря прочих слов
                    //TechDictionary dicTech = vec.Front;
                    //TechDictionary dicNoTech;

                    //int maxUseWordTechCount = 0;
                    //int maxUseWordNoTechCount = 0;


                    for (int i = 0; i < vec.Count; i++)
                    {

                        //for (int j = 0; j < vec.At(i).VectorPerDate.Count; j++)
                        {
                            //  if (vec.At(i).VectorPerDate.At(j).Date == wayToDictionaryPath)
                            {
                                if (vec[i].IsTech)
                                {
                                    VecTech.Add(vec[i]);
                                    //     if (vec.At(i).VectorPerDate.At(j).UsingTimes > maxUseWordTechCount)
                                    {
                                        //         maxUseWordTechCount = vec.At(i).VectorPerDate.At(j).UsingTimes;
                                    }
                                }
                                else
                                {
                                    //     if (vec.At(i).VectorPerDate.At(j).UsingTimes > maxUseWordNoTechCount)
                                    {
                                        VecNoTech.Add(vec[i]);
                                        //          maxUseWordTechCount = vec.At(i).VectorPerDate.At(j).UsingTimes;
                                    }
                                }
                            }
                        }


                    }
                    //vecTech = new(maxUseWordTechCount);
                    //vecNoTech = new(maxUseWordNoTechCount);
                    VecNoTech.Sort((y, x) => x.UsingTimes.CompareTo(y.UsingTimes));
                    VecTech.Sort((y, x) => x.UsingTimes.CompareTo(y.UsingTimes));

                    //for (int i = 0; i < vec.Count; i++)
                    {

                        //  for (int j = 0; j < vec.At(i).VectorPerDate.Count; j++)
                        {
                            //     if (vec.At(i).VectorPerDate.At(j).Date == wayToDictionaryPath)
                            {
                                // if (vec.At(i).IsTech)
                                {
                                    //           vecTech[vec.At(i).VectorPerDate.At(j).UsingTimes] = vec.At(i);
                                }
                                //else
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
