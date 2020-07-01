﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _01_多线程_分步渲染_提升渲染速度
{
    public partial class Form1 : Form
    {
        public static Form1 main;
        private Scene scene;
        private Renderer renderer;

        public Form1()
        {
            InitializeComponent();
            main = this;
            InitScene();
            renderer = new Renderer(scene,1000);
            
        }

        private void InitScene()
        {
            int width = 2000;
            int height = 1000;
            bool isSky = true;

            Vector3D lookFrom = new Vector3D(13, 2, 3);
            Vector3D lookAt = new Vector3D(0, 0, 0);
            float diskToFocus = (lookFrom - lookAt).Length();
            float aperture = 0;
            Camera camera = new Camera(lookFrom, lookAt, new Vector3D(0, 1, 0), 20,
                (float)width / (float)height, aperture, 0.7f * diskToFocus, 0, 1);

            List<Hitable> list = new List<Hitable>();
            list.Add(new Sphere(new Vector3D(0, -1000, 0), 1000, new Lambertian(new ConstantTexture(new Vector3D(0.5f, 0.5f, 0.5f)))));
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMat = Mathf.Randomfloat();
                    Vector3D center = new Vector3D(a + 0.9f * Mathf.Randomfloat(), 0.2f, b + 0.9f * Mathf.Randomfloat());
                    if ((center - new Vector3D(4, 0.2f, 0)).Length() > 0.9)
                    {
                        if (chooseMat < 0.8)
                        {
                            list.Add(new Sphere(center, 0.2f, new Lambertian(new ConstantTexture(new Vector3D(
                                                            Mathf.Randomfloat() * Mathf.Randomfloat(),
                                                            Mathf.Randomfloat() * Mathf.Randomfloat(),
                                                            Mathf.Randomfloat() * Mathf.Randomfloat())))));
                        }
                        else if (chooseMat < 0.95)
                        {
                            list.Add(new Sphere(center, 0.2f, new Metal(new Vector3D(
                                                                0.5f * (1 + Mathf.Randomfloat()),
                                                                0.5f * (1 + Mathf.Randomfloat()),
                                                                0.5f * (1 + Mathf.Randomfloat())),
                                                                0.5f * (1 + Mathf.Randomfloat()))));
                        }
                        else
                        {
                            list.Add(new Sphere(center, 0.2f, new Dielectric(1.5f)));
                        }
                    }
                }
            }
            list.Add(new Sphere(new Vector3D(0, 1, 0), 1, new Dielectric(1.5f)));
            list.Add(new Sphere(new Vector3D(-4, 1, 0), 1, new Lambertian(new ConstantTexture(new Vector3D(0.4f, 0.2f, 0.1f)))));
            list.Add(new Sphere(new Vector3D(4, 1, 0), 1, new Metal(new Vector3D(0.7f, 0.6f, 0.5f), 0)));
            BVHNode bb = new BVHNode(list, list.Count, 0, 1);
            HitableList world = new HitableList();
            world.list.Add(bb);
            scene = new Scene(width, height, world, isSky, camera,0,false);


        }
    }


}

