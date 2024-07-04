using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TextField> TextFields { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "8af10569-b018-4fe7-a380-7d6a14c70b74",
                Name = "admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "5e84bf2c-585f-42dc-a868-73157016ec70",
                Name = "moderator",
                NormalizedName = "MODERATOR"
            });
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "my@email.com",
                NormalizedEmail = "MY@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "superpassword"),
                SecurityStamp = string.Empty,
                CreateOn = DateTime.Now
            },
            new ApplicationUser
            {
                Id = "86d55f40-9544-4d92-aa24-cc5693a5fd96",
                UserName = "moderator",
                NormalizedUserName = "MODERATOR",
                Email = "moderator@email.com",
                NormalizedEmail = "MODERATOR@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "moderatorpassword"),
                SecurityStamp = string.Empty,
                CreateOn = DateTime.Now
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "8af10569-b018-4fe7-a380-7d6a14c70b74",
                UserId = "3b62472e-4f66-49fa-a20f-e7685b9565d8"
            },
            new IdentityUserRole<string>
            {
                RoleId = "5e84bf2c-585f-42dc-a868-73157016ec70",
                UserId = "86d55f40-9544-4d92-aa24-cc5693a5fd96"
            });

            builder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"),
                CodeWord = "PageIndex",
                Title = "Главная",
                Text = "<h2>Добро пожаловать в нашу библиотеку</h2><p>Эта <strong>Библиотека</strong> лучшая в мире и это не только мое мнение. Наша задача сделать литературу доступной для всех желающих. Предлагаем вам пройти регистрацию и начать пользоваться</p>"
            });
            builder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"),
                CodeWord = "PageBooks",
                Title = "Книги"
            });
            builder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"),
                CodeWord = "PageContacts",
                Title = "Контакты",
                Text = "<h2>Мои контакты</h2><p>Почта: cr.xeller@mail.ru</p><p>Телеграм: @NikitaGitNet</p>"
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("0d23f2ec-2b54-4dd9-b52b-b7c83a23dd0a"),
                Title = "Ересь Хоруса",
                SubTitle = "Там ересь была",
                IsBooking = false,
                Text = "Ваха 40к",
                TitleImagePath = "1655641005121914702.jpg",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Жафаров Ильнур",
                AuthorId = new Guid("ff9b30ce-ad2e-48ce-b811-e45481b55043")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("b2566eb6-2108-46ad-bc5f-b3a660d60d1b"),
                Title = "Ересь Хоруса: История Ангрона",
                SubTitle = "Там тоже была ересь",
                IsBooking = false,
                Text = "Ваха 40к",
                TitleImagePath = "1655641009118970804.jpg",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Букин Генадий",
                AuthorId = new Guid("c07ada61-d847-43eb-b2eb-8e32adcd64f4")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("f59c2a21-0f24-469a-b7cc-495146124310"),
                Title = "Повелитель мух",
                SubTitle = "История о мальчиках на острове",
                IsBooking = false,
                Text = "Муха делает ж-ж-ж-ж-ж-ж-ж-ж-ж-ж",
                TitleImagePath = "119029-uilyam-golding-povelitel-muh-119029.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Уильям Голдинг",
                AuthorId = new Guid("ebcbdb38-9d81-4cba-9f6b-b0edb5d6bffa")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("ba99c9bb-23fb-4238-8491-f11101b8b6fa"),
                Title = "Осень патриарха",
                SubTitle = "Про патриарха осенью",
                IsBooking = false,
                Text = "Что такое осень...",
                TitleImagePath = "119586-gabriel-garsia-markes-44998-osen-patriarha.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Габриэль Гарсиа",
                AuthorId = new Guid("e5c68c75-9b15-4424-b062-b91c7f4b6332")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("814f9c1c-9fef-42ba-acbe-857e234f31a8"),
                Title = "Метро 2033",
                SubTitle = "Про метро",
                IsBooking = false,
                Text = "Классика вселенной метро",
                TitleImagePath = "128391-dmitriy-gluhovskiy-metro-2033.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Дмитрий Глуховский",
                AuthorId = new Guid("4f91476b-4021-4581-b729-78c8fede5d3f")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("629555f5-a728-4d49-b9c5-754e06c5baef"),
                Title = "Зона поражения",
                SubTitle = "Сталкер",
                IsBooking = false,
                Text = "Классика вселенной сталкер",
                TitleImagePath = "152893-vasiliy-orehov-zona-porazheniya.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Василий Орехов",
                AuthorId = new Guid("9c37a892-1e81-4120-b8be-de6833215e67")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("e895336e-deb2-4a70-a5b6-38cbd1501507"),
                Title = "Сердце зоны",
                SubTitle = "Сталкер",
                IsBooking = false,
                Text = "Классика вселенной сталкер",
                TitleImagePath = "165057-andrey-levickiy-serdce-zony.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Андрей Левицкий",
                AuthorId = new Guid("d695cfa5-04cd-40ac-b0d7-307a8a876860")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("ebb65c65-bac4-4215-8c4b-dcda8faa6e25"),
                Title = "Свинцовый закат",
                SubTitle = "Сталкер",
                IsBooking = false,
                Text = "Классика вселенной сталкер",
                TitleImagePath = "172514-roman-glushkov-svincovyy-zakat-172514.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Роман Глушков",
                AuthorId = new Guid("fa10b14c-e352-4eac-91ae-982cb3fd3982")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("1cb4916a-5755-4784-bed0-a69db6311504"),
                Title = "Муос",
                SubTitle = "Про метро",
                IsBooking = false,
                Text = "Классика вселенной метро",
                TitleImagePath = "3748275-zahar-petrov-muos.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Захар Петров",
                AuthorId = new Guid("33a9c3e4-be82-40dd-8c06-4f29b0ac732a")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("058b6f8f-b5c6-4b4f-9124-2c213cb3edfa"),
                Title = "Атлант расправил плечи",
                SubTitle = "Про атланта",
                IsBooking = false,
                Text = "Ну там то про то, то про это",
                TitleImagePath = "4236675-ayn-rend-atlant-raspravil-plechi.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Айн Рэнд",
                AuthorId = new Guid("2acdaae8-d1ce-4995-bc07-537f77cc74bc")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("8236e6e4-f03c-48ff-849a-a1e7d0d48cae"),
                Title = "Под куполом",
                SubTitle = "Про изоляцию людей под куполом",
                IsBooking = false,
                Text = "Я ожидал большего от книги, честно говоря",
                TitleImagePath = "4387285-stiven-king-pod-kupolom.webp",
                GenreName = "Хоррор",
                GenreId = new Guid("e4ac715f-a600-4d01-8a09-5439c7b689b3"),
                AuthorName = "Стивен Кинг",
                AuthorId = new Guid("bb1a20f6-4406-4fe3-bf82-b3b31453667b")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("e89bd603-cda2-4bad-94d7-68862640a58d"),
                Title = "Темная башня",
                SubTitle = "Про башню наверн",
                IsBooking = false,
                Text = "Ну там Рендал Флегг есть",
                TitleImagePath = "4887784-stiven-king-temnaya-bashnya.webp",
                GenreName = "Хоррор",
                GenreId = new Guid("e4ac715f-a600-4d01-8a09-5439c7b689b3"),
                AuthorName = "Стивен Кинг",
                AuthorId = new Guid("bb1a20f6-4406-4fe3-bf82-b3b31453667b")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("1b12fa4b-ad1a-45e1-ba9a-c67ea054e317"),
                Title = "Сын полка",
                SubTitle = "Про мальчика",
                IsBooking = false,
                Text = "В полке мальчик был",
                TitleImagePath = "8217563-valentin-kataev-syn-polka-8217563.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Валентин Катаев",
                AuthorId = new Guid("afe48adb-b2a8-4d40-aef2-2816ec837304")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("45723e77-c301-4ac5-b6bc-2f5561fe453e"),
                Title = "Кто нашел, берет себе",
                SubTitle = "Вот нашел, забрал себе",
                IsBooking = false,
                Text = "Вроде норм, не читал",
                TitleImagePath = "12483328-stiven-king-kto-nashel-beret-sebe-12483328.webp",
                GenreName = "Хоррор",
                GenreId = new Guid("e4ac715f-a600-4d01-8a09-5439c7b689b3"),
                AuthorName = "Стивен Кинг",
                AuthorId = new Guid("bb1a20f6-4406-4fe3-bf82-b3b31453667b")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("c0a214e6-aa17-40ad-90fe-e1f465022102"),
                Title = "Билли Саммерс",
                SubTitle = "Про киллера",
                IsBooking = false,
                Text = "Аудиокнига у Булдакова еще не вышла, а печатную читать нет времени",
                TitleImagePath = "67303161-stiven-king-billi-sammers.webp",
                GenreName = "Хоррор",
                GenreId = new Guid("e4ac715f-a600-4d01-8a09-5439c7b689b3"),
                AuthorName = "Стивен Кинг",
                AuthorId = new Guid("bb1a20f6-4406-4fe3-bf82-b3b31453667b")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("893ed244-ed74-4618-b101-3e88b726d128"),
                Title = "О дивный новый мир",
                SubTitle = "Это шедевр!!!",
                IsBooking = false,
                Text = "Книга в свое время перевернувшая мое представление о совершенном мире",
                TitleImagePath = "14332383-oldos-leonard-haksli-o-divnyy-novyy-mir.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Олдос Хаксли",
                AuthorId = new Guid("a40e7075-54fc-4015-9488-1b32652b96e4")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("ba3af0e2-6a4c-4070-aa0c-0ffc0027d60f"),
                Title = "Скотный двор",
                SubTitle = "Книга о дескридетирующая коммунистический строй",
                IsBooking = false,
                Text = "Неплохо",
                TitleImagePath = "19258232--.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Джордж Оруэл",
                AuthorId = new Guid("a6ffa373-2b5a-424e-b5ed-835678fd0d92")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("0b6dbc1a-0ab8-4e00-b620-bf517abe8789"),
                Title = "Шерлок Холмс",
                SubTitle = "Классический детектив",
                IsBooking = false,
                Text = "Неплохо",
                TitleImagePath = "23571677-arthur-konan-doyle-ves-sherlok-holms.webp",
                GenreName = "Детектив",
                GenreId = new Guid("c500c41b-6b66-424b-b8d8-bf88e8f76a9e"),
                AuthorName = "Артур Конан Дойл",
                AuthorId = new Guid("7785baef-d027-4c00-be4f-237846c9c318")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("85b7a108-6e59-43ec-946f-39baf40d49fe"),
                Title = "Переход",
                SubTitle = "Про метро",
                IsBooking = false,
                Text = "Классика вселенной метро",
                TitleImagePath = "23789586-nail-vybornov-metro-2033-perehod.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Наиль Выборнов",
                AuthorId = new Guid("65b1fd9e-36d4-4965-bd3f-66a3e4859426")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("9326fb09-8557-4cdf-a2e5-6a3cf65ff252"),
                Title = "На западном фронте без перемен",
                SubTitle = "Про фронт книга",
                IsBooking = false,
                Text = "Нет, не про фронэнд",
                TitleImagePath = "32878920-erih-mariya-remark-na-zapadnom-fronte-bez-peremen-32878920.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Эрих Мария Ремарк",
                AuthorId = new Guid("ee09b74a-99ff-4f41-a04b-5957c2d8c3b6")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("8638a5fa-e30d-41a0-9db2-1493d38044eb"),
                Title = "451 градус по фарингейту",
                SubTitle = "По сути книга про то, к чему стремится наша поп культура",
                IsBooking = false,
                Text = "Мне не очень зашла",
                TitleImagePath = "39507162--.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Рей Брэдбери",
                AuthorId = new Guid("1198ab8d-59e8-4ce8-a4ee-98cce1f65eec")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("c3922ccf-dcfa-4c35-90bc-cc9dbc5e8902"),
                Title = "1984",
                SubTitle = "Лучшее что я читал",
                IsBooking = false,
                Text = "После себя оставляет ощущение опустошенности и безысходности",
                TitleImagePath = "67281231--.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Джордж Оруэл",
                AuthorId = new Guid("a6ffa373-2b5a-424e-b5ed-835678fd0d92")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("50259038-7e6f-402e-a80a-4a85a9cf8afe"),
                Title = "Лето волонтера",
                SubTitle = "Наверное про то как волотер провел лето",
                IsBooking = false,
                Text = "Не читал, не знаю о чем она",
                TitleImagePath = "67385403-sergey-lukyanenko-leto-volontera.webp",
                GenreName = "Приключения",
                GenreId = new Guid("87bbe432-9779-4488-a769-0c067a65aecd"),
                AuthorName = "Сергей Лукьяненко",
                AuthorId = new Guid("3cbe84ba-9744-4cef-9dd9-815c7c53e133")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("d0528761-f101-4f4f-b0fc-0a970a2026bd"),
                Title = "Выбор из худшего",
                SubTitle = "Типичный попаданец",
                IsBooking = false,
                Text = "Конкретно эту не читал, но у попаданцев в целом 30/70 - хорошие/плохие книги",
                TitleImagePath = "67891001-aleksey-grishin-vybor-iz-hudshego.webp",
                GenreName = "Приключения",
                GenreId = new Guid("87bbe432-9779-4488-a769-0c067a65aecd"),
                AuthorName = "Алексей Гришин",
                AuthorId = new Guid("3f40c815-4d3f-42f7-92e8-10543306465f")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("19f4d611-5a6a-4cf1-bcde-02e85081bb18"),
                Title = "В отсутствие начальства",
                SubTitle = "Российская классика",
                IsBooking = false,
                Text = "Даже не знаю, чем они там могли заняться, с работы раньше ушли наверное...",
                TitleImagePath = "67922364-nikolay-svechin-v-otsutstvie-nachalstva.webp",
                GenreName = "Мировая классика",
                GenreId = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                AuthorName = "Николай Свечин",
                AuthorId = new Guid("549ceae8-770c-4cd1-8ca8-c7f13bd7ba64")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("8fbc3dd1-8d19-454e-8aa3-a31749ea66be"),
                Title = "Истинный князь",
                SubTitle = "Один истинный, остальные ложные",
                IsBooking = false,
                Text = "Ну там сначала ложный был, потом истинный появился",
                TitleImagePath = "68010554-dmitriy-vitalevich-sheleg-istinnyy-knyaz.webp",
                GenreName = "Детектив",
                GenreId = new Guid("c500c41b-6b66-424b-b8d8-bf88e8f76a9e"),
                AuthorName = "Дмитрий Шелег",
                AuthorId = new Guid("5ac34184-f039-43da-8594-35e33c820882")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("94271f11-d6b7-43a8-b516-1b0f78f5cf10"),
                Title = "Исповедь экономического убийцы",
                SubTitle = "Про загнивающий запад",
                IsBooking = false,
                Text = "Обэма во всем виноват",
                TitleImagePath = "132973-dzhon-perkins-ispoved-ekonomicheskogo-ubiycy.webp",
                GenreName = "Научная литература",
                GenreId = new Guid("008fb99c-c826-4ee2-bf89-e60f1ffaf6a9"),
                AuthorName = "Джон Перкинс",
                AuthorId = new Guid("b573b21d-544f-4072-a595-b427b54247a2")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("213f74db-5ffd-4123-bd8c-e07c63c02c03"),
                Title = "Как дела, дорогой Карл?",
                SubTitle = "А как у него дела, кст?",
                IsBooking = false,
                Text = "Я хз, он не говорит",
                TitleImagePath = "67985997-virzhini-muza-kak-dela-dorogoy-karl-otkrovennye-memuary-telohranitelya-kar.webp",
                GenreName = UnknownGenre.Name,
                GenreId = new Guid(UnknownGenre.Id),
                AuthorName = "Себастьян Жондо",
                AuthorId = new Guid("a9cfc2d7-f079-4205-8750-c48acbd2f231")
            });
            builder.Entity<Book>().HasData(new Book
            {
                Id = new Guid("8c47f19c-a91e-4524-8004-220fa1734c25"),
                Title = "Черный ангел",
                SubTitle = "Довольно неплохой тайтл по сталкеру",
                IsBooking = false,
                Text = "В зоне зародилась новая аномалия",
                TitleImagePath = "166136-vyacheslav-shalygin-chernyy-angel.webp",
                GenreName = "Фантастика",
                GenreId = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                AuthorName = "Вячеслав Шалыгин",
                AuthorId = new Guid("30254b73-712a-48d4-bf0b-412216a90ade")
            });

            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("da589ab3-c70c-4d96-9ea9-867fedea69ff"),
                Name = "Фантастика"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("e4ac715f-a600-4d01-8a09-5439c7b689b3"),
                Name = "Хоррор"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("c500c41b-6b66-424b-b8d8-bf88e8f76a9e"),
                Name = "Детектив"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("87bbe432-9779-4488-a769-0c067a65aecd"),
                Name = "Приключения"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("79500809-b481-49a6-aeb6-869b86f9901e"),
                Name = "Мировая классика"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid("008fb99c-c826-4ee2-bf89-e60f1ffaf6a9"),
                Name = "Научная литература"
            });
            builder.Entity<Genre>().HasData(new Genre
            {
                Id = new Guid(UnknownGenre.Id),
                Name = UnknownGenre.Name
            });

            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("c07ada61-d847-43eb-b2eb-8e32adcd64f4"),
                Name = "Букин Генадий"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("ff9b30ce-ad2e-48ce-b811-e45481b55043"),
                Name = "Жафаров Ильнур"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid(UnknownAuthor.Id),
                Name = UnknownAuthor.Name
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("bb1a20f6-4406-4fe3-bf82-b3b31453667b"),
                Name = "Стивен Кинг"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("a6ffa373-2b5a-424e-b5ed-835678fd0d92"),
                Name = "Джордж Оруэл"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("ebcbdb38-9d81-4cba-9f6b-b0edb5d6bffa"),
                Name = "Уильям Голдинг"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("e5c68c75-9b15-4424-b062-b91c7f4b6332"),
                Name = "Габриэль Гарсиа"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("4f91476b-4021-4581-b729-78c8fede5d3f"),
                Name = "Дмитрий Глуховский"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("9c37a892-1e81-4120-b8be-de6833215e67"),
                Name = "Василий Орехов"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("d695cfa5-04cd-40ac-b0d7-307a8a876860"),
                Name = "Андрей Левицкий"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("30254b73-712a-48d4-bf0b-412216a90ade"),
                Name = "Вячеслав Шалыгин"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("fa10b14c-e352-4eac-91ae-982cb3fd3982"),
                Name = "Роман Глушков"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("33a9c3e4-be82-40dd-8c06-4f29b0ac732a"),
                Name = "Захар Петров"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("2acdaae8-d1ce-4995-bc07-537f77cc74bc"),
                Name = "Айн Рэнд"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("afe48adb-b2a8-4d40-aef2-2816ec837304"),
                Name = "Валентин Катаев"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("a40e7075-54fc-4015-9488-1b32652b96e4"),
                Name = "Олдос Хаксли"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("7785baef-d027-4c00-be4f-237846c9c318"),
                Name = "Артур Конан Дойл"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("65b1fd9e-36d4-4965-bd3f-66a3e4859426"),
                Name = "Наиль Выборнов"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("ee09b74a-99ff-4f41-a04b-5957c2d8c3b6"),
                Name = "Эрих Мария Ремарк"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("1198ab8d-59e8-4ce8-a4ee-98cce1f65eec"),
                Name = "Рей Брэдбери"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("3cbe84ba-9744-4cef-9dd9-815c7c53e133"),
                Name = "Сергей Лукьяненко"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("3f40c815-4d3f-42f7-92e8-10543306465f"),
                Name = "Алексей Гришин"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("549ceae8-770c-4cd1-8ca8-c7f13bd7ba64"),
                Name = "Николай Свечин"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("5ac34184-f039-43da-8594-35e33c820882"),
                Name = "Дмитрий Шелег"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("b573b21d-544f-4072-a595-b427b54247a2"),
                Name = "Джон Перкинс"
            });
            builder.Entity<Author>().HasData(new Author
            {
                Id = new Guid("a9cfc2d7-f079-4205-8750-c48acbd2f231"),
                Name = "Себастьян Жондо"
            });
        }
    }
}
