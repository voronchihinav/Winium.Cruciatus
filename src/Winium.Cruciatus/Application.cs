﻿namespace Winium.Cruciatus
{
    #region using

    using System;
    using System.Diagnostics;
    using System.IO;

    using Winium.Cruciatus.Exceptions;

    #endregion

    /// <summary>
    /// Класс для запуска и завершения приложения.
    /// </summary>
    public class Application
    {
        #region Fields

        private readonly string executableFilePath;

        private Process process;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Создает объект класса.
        /// </summary>
        /// <param name="executableFilePath">
        /// Полный путь до исполняемого файла.
        /// </param>
        public Application(string executableFilePath)
        {
            if (executableFilePath == null)
            {
                throw new ArgumentNullException("executableFilePath");
            }

            this.executableFilePath = executableFilePath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Посылает сообщение о закрытии главному окну приложения.
        /// </summary>
        /// <returns>
        /// true если приложение завершилось и false в противном случае.
        /// </returns>
        public bool Close()
        {
            this.process.CloseMainWindow();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Убивает приложение.
        /// </summary>
        /// <returns>
        /// true если приложение завершилось и false в противном случае.
        /// </returns>
        public bool Kill()
        {
            this.process.Kill();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Запускает исполняемый файл.
        /// </summary>
        public void Start()
        {
            if (!File.Exists(this.executableFilePath))
            {
                throw new CruciatusException("Неверно задан путь до исполняемого файла приложения.");
            }

            var directory = Path.GetDirectoryName(this.executableFilePath);

            // ReSharper disable once AssignNullToNotNullAttribute
            // directory не может быть null, в связи с проверкой выше наличия файла _exeFileName
            var info = new ProcessStartInfo { FileName = this.executableFilePath, WorkingDirectory = directory };
            this.process = Process.Start(info);
        }

        #endregion
    }
}
