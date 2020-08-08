using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMessagesNew2.Models.Database;
using EMessagesNew2.Classes.Utilities;
using EMessagesNew2.Enums;
using EMessagesNew2.Extensions;
using EMessagesNew2.Models.Configurations;
using System.Text;

namespace EMessagesNew2.Classes.Database
{
    /// <summary>
    /// Главный контекст базы данных
    /// </summary>
    public class MainContext : DbContext
    {
        /// <summary>
        /// Представляет пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Представляет авторизационные данные пользователей
        /// </summary>
        public DbSet<Auth> Auth { get; set; }

        /// <summary>
        /// Представляет чаты
        /// </summary>
        public DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// Представляет сообщения
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Представляет сессии ключей
        /// </summary>
        public DbSet<KeySession> KeySessions { get; set; }

        /// <summary>
        /// Представляет зашифрованные данные чатов
        /// </summary>
        public DbSet<EncryptionChatData> EncryptionChatDatas { get; set; }

        /// <summary>
        /// Представляет контакты пользователей
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Представляет пары зашифрованный приватный ключ / публичный ключ
        /// </summary>
        public DbSet<RSAKeyPair> RSAKeyPairs { get; set; }

        /// <summary>
        /// Представляет зашифрованные публичные ключи других пользователей
        /// </summary>
        public DbSet<EncryptedPublicKey> EncryptedPublicKeys { get; set; }

        /// <summary>
        /// Представляет уведомления для пользователей
        /// </summary>
        public DbSet<UserEvent> UserEvents { get; set; }

        /// <summary>
        /// Представляет информацию о файлах
        /// </summary>
        public DbSet<FileData> Files { get; set; }

        /// <summary>
        /// Создает экземпляр базы данных и проверяет ее целостность
        /// </summary>
        /// <param name="options">Экземпляр <see cref="DbContextOptions"/></param>
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception e)
            {
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEventParam>().HasOne(p => p.UserEvent).WithMany(t => t.Params).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
