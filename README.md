Загрузка программы 

Загрузка заключается в установке архива, содержащего программный продукт по открытой ссылке: https://github.com/sevastyanova2003/sevas . 

Запуск программы 

Серверная часть требует запуска сервера PostgreSQL, поэтому перед началом работы 
необходимо убедиться, что на вашем устройстве установлен PostgreSQL, а также pgAdmin.  

Так как сервер разворачивается локально на компьютере, необходимо  создать базу данных codeanalyzerdb (через pgAdmin) и в CodeAnalyzerServer/CodeAnalyzerServer/Program.cs connectionString, используя свой пароль: коде в серверной 8ой строке части изменить в файле значение

string connectionString = "Host=localhost;Username=postgres;Password=yourPassword;Database=codeanalyzerdb"; 

При желании сменить логин и пароль админа (по умолчанию это admin и 123) необходимо 
в файле CodeAnalyzerServer/CodeAnalyzerServer/RequestHandler.cs в 15-16 строках изменить 
значения  adminUsername и  adminPassword на предпочтительные: 

string adminUsername = "adminName"; 

string adminPassword = "adminPassword"; 

Далее необходимо запустить сервер (папка  CodeAnalyzerServer). 
После всех приготовлений можно запускать клиентскую часть (папка CodeAnalyzerWinForms).

В папке CodeAnalyzerTests лежат примеры файлов, на которых программа тестировалась (скриншоты в пояснительной записке)
