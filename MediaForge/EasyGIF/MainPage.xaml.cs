﻿using MForge.Sequensor.Sequence;
using MForge.Sequensor.Sequence.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace EasyGIF
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();            

            scenes.Controller.SceneChanged += (controller, scene) =>
            {
                sequensor.SetScene(scene);
            };
        }

        private void DeleteDropOver(object sender, DragEventArgs e)
        {
            var context = e.Data.Properties["Context"];
            if (context is ISequence)
            {
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            }
            else if (context is IScene)
            {
                if (scenes.Controller.Scenes.Count > 1)
                {
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                }
            }
        }

        private void Drop(object sender, DragEventArgs e)
        {
            var context = e.Data.Properties["Context"];
            if (context is ISequence)
            {
                sequensor.DeleteSequence(context as ISequence);
            }
            else if (context is IScene)
            {
                scenes.DeleteScene(context as IScene);
            }                
        }
    }
}
