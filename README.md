Переписал с нуля свой старый проект Ellib, исправил косяки, переделал архитектуру, логику и многое другое

ТЗ:
Разработать веб-сервис "Библиотека".
3 роли: администратор, библиотекарь, клиент

Администратор:
Уудалять пользователей, устанавливать пароли.

Библиотекарь:
Может добавлять и удалять книги
Может выдавать и принимать книги от клиентов

Клиент:
Может просматривать имеющиеся книги
Искать по автору, жанру, издателю
Бронировать книги, снимать бронь

Бизнес-процесс:
Клиент заходит на сервис, бронирует книгу, затем приходит в библиотеку.
Далее библиотекарь выдает книгу клиенту.
Через некоторое время библиотекарь принимает книгу от клиента.
Забронированную и выданную книгу нельзя забронировать и выдать
Время бронирования ограничено.
Приветствуются дополнительные функции, например: Возможность оценивать книги и писать комментарии о книге, чтобы другие пользователи могли видеть их.

Стек технологий:
Visual Studio 2019+ / JetBrains Rider 
ASP.Net Core 3.1+
Entity Framework Core 3.0+ / Dapper
PostgreSQL 10+
Использовать IoC Autofac или стандартный MS DI
React / Razor pages на выбор
