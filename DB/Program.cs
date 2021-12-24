using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Parser;
using Parser.Python;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Parser.CSAnalizator;
using LemmaSharp;
using LemmaSharp.Classes;
using DeepMorphy;
using Parser.Infrastructure.Python;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DB
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public Person Parent { get; set; }
    }

    public class Setter
    {
        public string SourceType { get; }
        public Setter(CrawlableSource source)
        {
            SourceType = source.GetType().Name;
            Source = source;
        }

        public CrawlableSource Source { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Source);
        }

        public PageArchitectureSite ToSource(string json)
        {
            return JsonConvert.DeserializeObject<PageArchitectureSite>(json);
        }
    }


    public class Try<T>
    {
        public Try(Func<T,string> func)
        {
            Print = func;
        }
        private Func<T, string> Print;
        private Func<T, string> GetPrint()
        {
            return Print;
        }
    }

   

    class Program
    {
        public static Expression<Func<TOwner, TPropType>> Excluding<TOwner, TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            return memberSelector;
        }

        
        public static void UnPush(object obj, Dictionary<object,Tuple<Type,Type>> dict)
        {
            var t = dict[obj];

            Console.WriteLine(t.Item1);
            Console.WriteLine(t.Item2);

            var f = (Func< Person,string>)obj;
            Console.WriteLine(f.Invoke(new Person() { Name = "Tttoha", Age = 33, Height = 1.5 }));
        }

        static string GetNameFromMemberExpression(Expression expression) {
		if (expression is MemberExpression) {
			return (expression as MemberExpression).Member.Name;
		}
		else if (expression is UnaryExpression) {
			return GetNameFromMemberExpression((expression as UnaryExpression).Operand);
		}
		return "MemberNameUnknown";
	    }

        public static string GetInfo<T, P>(Expression<Func<T, P>> action)
        {
            var expression = (MemberExpression)action.Body;
            string name = expression.Member.Name;

            return name;
        }

        private static string PrintToString(string parentPath, object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();


            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                string newPath;
                if (string.IsNullOrEmpty(parentPath))
                    newPath = $"{propertyInfo.Name}";
                else
                    newPath = $"{parentPath}.{propertyInfo.Name}";
                Console.WriteLine(newPath);

                sb.Append(identation + propertyInfo.Name + " = " +
                        PrintToString(newPath, propertyInfo.GetValue(obj),
                            nestingLevel + 1));

            }

            return sb.ToString();
        }

        /*private static void PrintToString(string parentPath, object obj, int nestingLevel)
        {
            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (obj == null || finalTypes.Contains(obj.GetType()))
            {
                return;
            }

            var type = obj.GetType();
            foreach (var propertyInfo in type.GetProperties())
            {
                string newPath;
                if(string.IsNullOrEmpty(parentPath))
                    newPath = $"{propertyInfo.Name}";
                else
                    newPath = $"{parentPath}.{propertyInfo.Name}";
                Console.WriteLine(newPath);
                PrintToString(newPath, propertyInfo.GetValue(obj), nestingLevel + 1);
            }
        }*/


        public static string Foo<T, P>(Expression<Func<T, P>> expr)
        {
            var result = new Stack<string>();
            MemberExpression me = (MemberExpression)expr.Body;

            while (me != null)
            {
                string propertyName = me.Member.Name;

                result.Push(propertyName);

                me = me.Expression as MemberExpression;
            }

            return string.Join(".", result);
        }


        static async Task Main(string[] args)
        {
            var str = @" Коррупция является экономической, политической и социальной проблемой, поскольку выводит из оборота значительные средства, снижает уровень доверия граждан к власти, замедляет темпы роста качества жизни больших групп населения. Плохо сдерживаемая коррупция перечёркивает положительные достижения в экономической сфере и социальной жизни.Практика свидетельствует, что противодействие коррупции не может сводиться только к привлечению к ответственности лиц, виновных в коррупционных нарушениях, необходима система правовых, экономических, образовательных, воспитательных, организационных и иных мер, направленных на предупреждение коррупции, устранение причин, ее порождающих.Противодействие коррупции является важнейшей стратегической задачей деятельности органов государственной власти, органов местного самоуправления, организаций, институтов гражданского общества, юридических и физических лиц в рамках их полномочий по профилактике и борьбе с коррупцией, а также ликвидации и минимизаций последствий коррупционных правонарушений.В Российской Федерации сформирована правовая база антикоррупционной политики, определяющая полномочия, формы и методы работы органов государственной власти, органов местного самоуправления, институтов гражданского общества по противодействию коррупции.Нормативными правовыми актами, определяющими взаимодействие органов государственной и муниципальной власти в противодействии коррупции, являются:1) Федеральный закон от 25 декабря 2008 года № 273-ФЗ «О противодействии коррупции»;2) Федеральный закон от 7 февраля 2011 года № 3-ФЗ «О полиции»;3) Указ Президента Российской Федерации от 19.05.2008 № 815 «О мерах по противодействию коррупции»;4) Указ Президента Российской Федерации от 13.04.2010 № 460 «О Национальной стратегии противодействия коррупции и Национальном плане противодействия коррупции на 2010 - 2011 годы»;5) Указы Президента Российской Федерации «О Национальном плане противодействия коррупции на периоды годов».В целях реализации федерального антикоррупционного законодательства в Свердловской области приняты региональные нормативные правовые акты, определяющие основные направления взаимодействия институтов гражданского общества и органов власти в сфере профилактики и противодействия коррупции, и предусматривающие соответствующие организационные меры по повышению гражданской (общественной) активности в реализации мероприятий в сфере профилактики и противодействия коррупции:1) Закон Свердловской области от 20 февраля 2009 года № 2-ОЗ «О противодействии коррупции в Свердловской области»;2) Указ Губернатора Свердловской области от 09.10.2015 № 449-УГ «О Комиссии по координации работы по противодействию коррупции в Свердловской области»;3) Указ Губернатора Свердловской области от 29.07.2016 № 441-УГ «О рабочей группе по взаимодействию с институтами гражданского общества при Комиссии по координации работы по противодействию коррупции в Свердловской области».Основной формой взаимодействия правоохранительных органов и органов власти в противодействии коррупции являются участие в работе консультативно-совещательные органов, формируемых при органах власти всех уровней.Указом Губернатора Свердловской области от 09.10.2015 № 449-УГ «О Комиссии по координации работы по противодействию коррупции в Свердловской области» образована Комиссия по координации работы по противодействию коррупции в Свердловской области (далее – Комиссия), председателем которой является Губернатор Свердловской области Е.В. Куйвашев.Одной из основных задач деятельности Комиссии является обеспечение взаимодействия исполнительных органов государственной власти Свердловской области, иных государственных органов Свердловской области и органов местного самоуправления муниципальных образований, расположенных на территории Свердловской области, с гражданами, институтами гражданского общества, средствами массовой информации, научными организациями по вопросам противодействия коррупции в Свердловской области.В целях обеспечения взаимодействия в состав Комиссии включены представители правоохранительных органов, а также представители Общественной палаты Свердловской области, малого и среднего бизнеса Свердловской области, Федерации профсоюзов Свердловской области.При Комиссии по координации работы по противодействию коррупции в Свердловской области создана рабочая группа по взаимодействию с институтами гражданского общества. Основной задачей рабочей группы является осуществление взаимодействия исполнительных органов государственной власти Свердловской области, иных государственных органов Свердловской области и органов местного самоуправления муниципальных образований, расположенных на территории Свердловской области, с гражданами, институтами гражданского общества, средствами массовой информации, научными организациями по вопросам противодействия коррупции в Свердловской области.Следующим направлением работы по взаимодействию правоохранительных органов и органов исполнительной власти Свердловской области по противодействию коррупции и профилактике коррупционных правонарушений является участие в совместных мероприятиях, организуемых и проводимых органами государственной власти, предусмотренных Планами работы органов государственной власти Свердловской области по противодействию коррупции. Например, представители управления экономической безопасности и противодействия коррупции ежегодно принимают участие в Антикоррупционном форуме.Следующее направление взаимодействия – это направление информации о фактах склонения государственных служащих к совершению коррупционных правонарушений и преступлений. Однако факты направления такой информации в органы внутренних дел носят единичный характер.В Свердловской области сложилась действенная система взаимодействия органов государственной власти, органов местного самоуправления и правоохранительных органов в сфере противодействия коррупции.Конструктивное взаимодействие ГУ МВД России по Свердловской области и органов государственной власти Свердловской области, органов местного самоуправления муниципальных образований, расположенных на территории Свердловской области, в сфере противодействия коррупции является одним из ведущих приоритетов антикоррупционной политики на Среднем Урале.В 2021 году мероприятия по реализации задач, поставленных перед органами внутренних дел по противодействию коррупции, проводятся в соответствии с Федеральным законом от 25.12.2008 № 273-ФЗ «О противодействии коррупции», Указом Президента Российской Федерации от 13.04.2010 № 460 «О Национальной стратегии противодействия коррупции», Национальным планом противодействия коррупции на 2021 – 2024 года, утвержденным Указом Президента Российской Федерации от16.08.2021 № 478, и планом МВД России по противодействию коррупции на 2021 – 2024 годы.В ходе проведенных мероприятий по противодействию коррупции за 11 месяцев 2021 года (по состоянию на 29.11.2021) органами внутренних дел Свердловской области выявлено 248 преступлений коррупционной направленности, из которых 236 преступлений совершены по тяжким и особо тяжким составам, 64 совершено в крупном и особо крупном размере. Окончено расследование уголовных дел по 259 преступлениям данной категории, в суд направлены уголовные дела по 187 преступлениям, к уголовной ответственности привлечено 128 лиц.Основными видами коррупционной преступности являются:– хищение бюджетных денежных средств или средств организаций, направленных на перечисление в бюджетные организации;– получение взятки работниками муниципальных учреждений и коммерческих организаций;– внесение заведомо ложных сведений в официальные документы.Например, 02.03.2021 возбуждено уголовное дело по признакам преступления, предусмотренного ч. 3 ст. 159 УК РФ в отношении Т. и П., которые действуя от имени различных коммерческих организаций, из корыстных побуждений, посредством предоставления в ФГБУ «Фонд содействия развитию малых форм предприятий в научно-технической сфере» заведомо ложных, несоответствующих действительности документов о понесенных расходах на выполнение научно-исследовательских и опытно-конструкторских работ», в рамках исполнения государственных контрактов, похитили бюджетные средства, выделенные в качестве субсидии, в особо крупном размере в сумме свыше 30 млн рублей.29.03.2021 возбуждено уголовное дело по признакам преступления, предусмотренного п. «а», «г» ч. 7 ст. 204 УК РФ в отношении М. по факту получения незаконного вознаграждения в размере 150 тыс. рублей за оказание содействия при заключении договора на поставку углесодержащих и шлакообразующих материалов.Наиболее опасным проявлением коррупции являются преступления против государственной власти, интересов государственной службы и службы в органах местного самоуправления (глава 30 УК РФ).В отчетный период выявлено 153 преступления экономической направленности по главе 30 УК РФ, следствие по которым обязательно, в т. ч. 4 факта злоупотребления должностными полномочиями (ст. 285 УК РФ) и 7 фактов превышения должностных полномочий (ст. 286 УК РФ). Выявлено 129 преступлений по фактам взяточничества, в т. ч. 25 в крупном и особо крупном размере.Средний размер взятки составил 113,1 тыс. рублей, в т. ч. по ст. 290 УК РФ – 156,2 тыс. рублей, ст. 291 УК РФ – 89,3 тыс. рублей, ст. 291.1 УК РФ – 104,4 тыс. рублей.В суд направлены уголовные дела по 118 преступлениям против государственной власти в отношении 59 лиц.Особое внимание, при осуществлении оперативно-разыскной деятельности, уделяется выявлению и привлечению к ответственности высокопоставленных коррупционеров и взяточников из числа чиновников государственной федеральной и муниципальной власти, контролирующих и надзорных органов.Так, 19.02.2021 возбуждено уголовное дело по признакам преступления, предусмотренного ч. 1 ст. 286 УК РФ в отношении должностных лиц одного из муниципалитетов по факту подписания актов фактически невыполненных работ по контракту на строительство школы на сумму более 50 млн рублей с целью получения из бюджета федеральных средств по программе «Устойчивое развитие сельских территорий».31.03.2021 возбуждено уголовное дело по признакам преступления, предусмотренного п. «в» ч. 5 ст. 290 УК РФ в отношении К., который в период времени с 01.04.2016 по 31.01.2017 получил незаконное вознаграждение в виде денежных и материальных средств, а также различных услуг на общую сумму не менее 548 тыс. рублей, за общее покровительство.31.03.2021 возбуждено уголовное дело по признакам преступления, предусмотренного ч. 6 ст. 290 УК РФ в отношении С., который получил незаконное денежное вознаграждения в размере не менее 1 млн рублей за общее покровительство, в части непринятия мер контроля.31.03.2021 возбуждено уголовное дело по признакам преступления, предусмотренного п. «в» ч. 5 ст. 290 УК РФ в отношении главного специалиста администрации одного из район7ов Екатеринбурга по факту получения взятки в сумме 328 000 рублей от предпринимателей за согласование размещения твердых бытовых отходов на земельных участках.30.06.2021 возбуждено уголовное дело по признакам преступлений, предусмотренных ч. 1 ст. 285, ч. 1 ст. 292 УК РФ в отношении К., которая используя должностные полномочия, действуя из личной заинтересованности, внесла в официальные документы заведомо ложные сведения об осмотре наличия транспортных средств, что явилось основанием для заключения муниципального контракта на выполнение работ по регулярным пассажирским перевозкам по городским маршрутам.11.08.2021 возбуждено уголовное дело по признакам преступления, предусмотренного п. «в» ч. 5 ст. 290 УК РФ, п. «б» ч. 4 ст. 291 УК РФ в отношении Ш., который в рамках осуществления контроля за выполнением договора подряда на выполнение работ по объекту ЖК, получил взятку в сумме 565 000 рублей за подписание актов о приемке выполненных работ.В августе 2021 года возбуждено 5 уголовных дел по признакам преступления, предусмотренного п. «а» ч. 5 ст. 290 УК РФ, в отношении Ш., который совместно с сообщником за внесение заведомо ложных сведений в протокол осмотра объекта недвижимости, т. е. за незаконные действия в нарушение п. «г» п. 4.2 ст. 9 Федерального закона от 08.08.2001 № 129-ФЗ «О государственной регистрации юридических лиц и индивидуальных предпринимателей», неоднократно получал взятки путем совершения безналичного перевода.В текущем году возбуждено 12 уголовных дел по ст. 290 УК РФ, в том числе 2 в крупном размере, в отношении П. по фактам получения взятки за совершение незаконных действий по вынесению постановления об окончании исполнительного производства без фактического их исполнения.Главным управлением продолжена реализация мероприятий по возмещению материального ущерба, причиненного преступлениями коррупционной направленности.Личный состав нацелен на своевременную реализацию мер по обеспечению установления и ареста имущества подозреваемых и обвиняемых, а также их близкого окружения, в совершении коррупционных преступлений для обеспечения гражданского иска, возмещения причиненного ущерба, а также возможной конфискации имущества. На данной стадии обеспечено проведение мероприятий, направленных на сбор информации о наличии (отсутствии) у лиц, в отношении которых проводится проверка, недвижимого имущества, денежных средств на счетах в банках, с целью принятия должных мер по его обнаружению и изъятию.С целью изучения проблем в практической работе по данному направлению, выработки единых подходов и путей их решения, сотрудниками ГУ на постоянной основе осуществляется взаимодействие с Федеральной службой по финансовому мониторингу, Федеральной службой государственной регистрации, кадастра, другими органами, располагающими сведениями об имуществе, подлежащего конфискации.Сумма возмещенного ущерба по преступлениям коррупционной направленности составила 92 млн 908 тыс. рублей.";


            var dbContext = new MyDBContext();

            var events = await dbContext.GetAllEventsAsync();

            foreach (var ev in events)
                Console.WriteLine(ev.Link);

            var analyzer = new DistrictAnalyzer(dbContext.Districts, dbContext.Addresses.Include(adr => adr.District));

            if (events.Any())
            {
                var newEvent = new Event();
                newEvent.Body = str;
                newEvent.IncidentCategory = "HP";
                newEvent.Link = "my link1";

                var distr = await analyzer.AnalyzeDistrict(newEvent.Body);
                //newEvent.District = distr;
                await dbContext.AddEventAsync(newEvent);
            }

            await dbContext.SaveChangesAsync();

            /* var dbContext = new MyDBContext();
             var analyzer = new DistrictAnalyzer(dbContext.Districts, dbContext.Addresses.Include(adr=>adr.District));

             foreach (var d in dbContext.Districts)
                 Console.WriteLine(d.DistrictName);

             var text = @"\r\n\t\t\t\t\t\t\tВ Свердловской области придется сжечь партии краснодарских семян моркови и укропа и партию семян кориандра из Московской области.Специалисты Свердловского референтного центра Россельхознадзора изучили образцы расфасованных в пакеты семян овощных культур, которые направило на испытания свердловское Управление Россельхознадзора.В исследованных образах были выявлены семена карантинных сорных растений.Семена сорняка Ambrosia artemisiifolia(амброзия полыннолистная) были обнаружены в образцах с семенами моркови сортов «Витаминная 6», «Лосиноостровская - 13», «Деликатесная» и «Нантская - 4», а также в семенах укропа сорта «Обильнолистный». Эти семена прибыли из Краснодарского края.В семенах кориандра сорта «Прелесть», привезенных из Московской области, были обнаружены семена карантинного сорняка повилики рода Cuscuta L.\r\n\t\t\t\t\t\t\tПолучать доступ к эксклюзивным и не только новостям «Вечерних ведомостей» быстрее можно, подписавшись на нас в сервисах «Яндекс.Новости» и «Google Новости».\r\n\t\t\t\t\t\t\tА получать информацию оперативнее всего и в более непринуждённой форме можно в нашем телеграм-канале.Там свежо, дерзко и есть много того, что мы не публикуем на сайте!Подписывайтесь!\r\n\t\t\t\t\t\t\tЕлизавета Свердлова &copy; &nbsp; Вечерние & nbsp; ведомости\r\n\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t";


             var distr = analyzer.AnalyzeDistrict(text);
             Console.WriteLine(distr.DistrictName);*/


            /* var districts = new List<string>()
             {
                 "академический",
                 "верх-исетский",
                 "железнодорожный",
                 "кировский",
                 "ленинский",
                 "октябрьский",
                 "орджоникидзевский",
                 "чкаловский"
             };
             //var morph = new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true);



             var categories = new Dictionary<string, HashSet<string>>()
                 {
                     {"ДТП" , new HashSet<string>() {"протаранить", "проехать", "наехать", "сбить", "выехать", "вылететь", "столкнуться", "врезаться", "авария" , "автомобиль"}},
                     {"Пожар" , new HashSet<string>() {"сгореть", "загореться" , "огонь", "пламя", "потушить", "воспламяниться", "гореть", "пожар", "вспыхнуть", "взорваться", "возгорание", "задымление", "дым", "газ"}},
                     {"Криминал" , new HashSet<string>() {"украсть", "ограбить", "ударить", "избить", "убить", "убийство", "кража", "труп", "тело" }},
                 };
             var analizator = new Analizator(categories, new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true));
             var category = await analizator.AnalizeCategoryAsync("Саня сосет бибу", "Не ЧП");

             Console.WriteLine(category);*/



            /*using (var reader = new StreamReader(@"D:\c#\RedZone\districs\streets_list_with_districts.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var streat = values[1];
                    var district = values[2];
                    var districtName = district.Split(" ");
                    if (districtName.Length == 2)
                        Console.WriteLine(districtName[1].Trim());
                    else
                        Console.WriteLine(district.Trim());
                    listA.Add(streat);
                    listB.Add(district);
                    Console.WriteLine(streat.Trim());
                }
            }*/


            /* var csvTable = new DataTable();
             using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"D:\c#\RedZone\districs\streets_list_with_districts.csv")), true))
             {
                 csvTable.Load(csvReader);
             }


             for (int i = 0; i < csvTable.Rows.Count; i++)
             {
                 Console.WriteLine($"{csvTable.Rows[i][0]} ");
             }

             Console.WriteLine(csvTable.Rows.Count);
             Console.WriteLine(csvTable.Columns.Count);*/


            /* var dir = new Dictionary<object, Tuple<Type, Type>>();

             var f = Excluding<Person, string>(x => x.Name).Compile();

             dir[f] = Tuple.Create(typeof(Person), typeof(string));

             UnPush(f, dir);*/

            /*Console.WriteLine(dir[f]);

            var r = f.Compile();
            var person = new Person() { Name = "Toha", Age = 22 };
            var personProperty = person.GetType().GetProperties().Where(x => x.Name != f.Name);

            var exceptProperty = r.Invoke(person);

            Console.WriteLine(f.GetType().Name);
            foreach(var p in personProperty)
            {
                Console.WriteLine(p.Name);
            }*/




            /*  System.Globalization.CultureInfo EnglishCulture = new
  System.Globalization.CultureInfo("en-EN");
              System.Globalization.CultureInfo GermanCulture = new
              System.Globalization.CultureInfo("de-de");

              double val;
              if (double.TryParse("65,89875", System.Globalization.NumberStyles.Float,
              GermanCulture, out val))
              {
                  string valInGermanFormat = val.ToString(GermanCulture);
                  string valInEnglishFormat = val.ToString(EnglishCulture);
                  Console.WriteLine(valInGermanFormat);
                  Console.WriteLine(valInEnglishFormat);
              }

              if (double.TryParse("65.89875", System.Globalization.NumberStyles.Float,
              EnglishCulture, out val))
              {
                  string valInGermanFormat = val.ToString(GermanCulture);
                  string valInEnglishFormat = val.ToString(EnglishCulture);
                  Console.WriteLine(valInGermanFormat);
                  Console.WriteLine(valInEnglishFormat);
              }*/
            /*var crawler = new Crawler();

            var curEvents = await dbContext.GetAllEventsAsync();
            Console.WriteLine(curEvents.Count);
            foreach (var e in curEvents)
                Console.WriteLine(e.Link);


            var sources = await dbContext.GetSourcesAsync();

            foreach (var s in sources)
                Console.WriteLine($"{s.Events.Count} {s.Fields.Properties}");
            var counter = 0;
            await foreach (var e in crawler.StartAsync(sources))
            {
                counter++;
                Console.WriteLine(counter.ToString());
                Console.WriteLine(e.Link);
                await dbContext.AddEventAsync(e);
            }

            dbContext.SaveChanges();*/

            #region
            /*List<CrawlableSource> sourceList = new List<CrawlableSource>()
             {
                   

             *//*  //only bad status code
             new PageArchitectureSite()
             {
                 StartUrl = "https://ekaterinburg.bezformata.com/incident/?npage=",
                 LinkURL = "",
                 EndUrl="",
                 LinkElement = new HtmlElement
                 {
                     XPath = @".//article[@class='listtopicline']/a",
                     AttributeName = "href"
                 },

                 ParseEventProperties = new Dictionary<string, string>
                 {
                     { "Body", ".//div[@class='article__body']//p" },
                     { "Date", ".//time[@class='meta__text']" }
                 }
             },*/

            /* //Робит(страница не очень красиво парсится)
              new PageArchitectureSite()
              {
                  StartUrl = "https://veved.ru/eburg/news/page/",
                  LinkURL = "",
                  EndUrl="",
                  LinkElement = new HtmlElement
                  {
                      XPath = @".//a[@class='box']",
                      AttributeName = "href"
                  },

                  ParseEventProperties = new Dictionary<string, string>
                  {
                      { "Body", ".//div[@class='fullstory-column']" },
                      { "Date", ".//div[@class='vremya']" }
                  }
              },*/

            /*new PageArchitectureSite()
            {
                StartUrl = "https://ural-meridian.ru/news/category/sverdlovskaya-oblast/page/",
                LinkURL = "",
                EndUrl="/",
                LinkElement = new HtmlElement
                {
                    XPath = @".//h2[@class='entry-title']/a",
                    AttributeName = "href"
                },

                ParseEventProperties = new Dictionary<string, string>
                {
                    { "Body", ".//div[@class='entry-content clear']" },
                    { "Date", ".//span[@class='published']" }
                }
            },*/

            /* //Робит
             new PageArchitectureSite()
             {
                 StartUrl = "https://sverdlovsk.sledcom.ru/Novosti/",
                 LinkURL = "https://sverdlovsk.sledcom.ru",
                 EndUrl="/?year=&month=&day=&type=main",
                 LinkElement = new HtmlElement
                 {
                     XPath = @".//div[@class='bl-item-image']/a",
                     AttributeName = "href"
                 },

                 ParseEventProperties = new Dictionary<string, string>
                 {
                     { "Body", ".//article[@class='c-detail m_b4']//p" },
                     { "Date", ".//div[@class='bl-item-date m_b2']" }
                 }
             },*/

            /* //Робит
              new PageArchitectureSite()
              {
                  StartUrl = "https://66.xn--b1aew.xn--p1ai/news/1",
                  LinkURL = "https://66.xn--b1aew.xn--p1ai",
                  EndUrl="",
                  LinkElement = new HtmlElement
                  {
                      XPath = @".//div[@class='sl-item-title']/a",
                      AttributeName = "href"
                  },

                  ParseEventProperties = new Dictionary<string, string>
                  {
                      { "Body", ".//div[@class='article']//p" },
                      { "Date", ".//div[@class='article-date-item']" }
                  }
              },*//*

             //Робит
             *//*  new PageArchitectureSite()
              {
                  StartUrl = "https://eburg.mk.ru/news/",
                  LinkURL = "",
                  EndUrl="/",
                  LinkElement = new HtmlElement
                  {
                      XPath = @".//a[@class='news-listing__item-link']",
                      AttributeName = "href"
                  },

                  ParseEventProperties = new Dictionary<string, string>
                  {
                      { "Body", ".//div[@class='article__body']//p" },
                      { "Date", ".//time[@class='meta__text']" }
                  }
              },*//*

          };

             var crawler = new Crawler();
             var set = new HashSet<string>();
             var counter = 1;
             await foreach (var s in crawler.StartAsync(sourceList))
             {
                 Console.WriteLine(counter);
                 counter++;
                 set.Add(s.Link);
                 Console.WriteLine(s.Body);
             }

             Console.WriteLine(set.Count);
             foreach (var l in set)
                 Console.WriteLine(l);*/

            /* var sources = new List<Source>
             {
                 new Source()
                 {
                     SourceType = SourceType.PageSite,
                     Fields = new SourceFields()
                     {
                         Properties = JsonConvert.SerializeObject
                         (
                           new PageArchitectureSite()
                          {
                              StartUrl = "https://veved.ru/eburg/news/page/",
                              LinkURL = "",
                              EndUrl="",
                              LinkElement = new HtmlElement
                              {
                                  XPath = @".//a[@class='box']",
                                  AttributeName = "href"
                              },

                              ParseEventProperties = new Dictionary<string, string>
                              {
                                  { "Body", ".//div[@class='fullstory-column']" },
                                  { "Date", ".//div[@class='vremya']" }
                              }
                          }
                         )
                     }

                 },
                 new Source()
                 {
                     SourceType = SourceType.PageSite,
                     Fields = new SourceFields()
                     {
                         Properties = JsonConvert.SerializeObject
                         (
                             new PageArchitectureSite()
                              {
                                  StartUrl = "https://ural-meridian.ru/news/category/sverdlovskaya-oblast/page/",
                                  LinkURL = "",
                                  EndUrl="/",
                                  LinkElement = new HtmlElement
                                  {
                                      XPath = @".//h2[@class='entry-title']/a",
                                      AttributeName = "href"
                                  },

                                  ParseEventProperties = new Dictionary<string, string>
                                  {
                                      { "Body", ".//div[@class='entry-content clear']/p" },
                                      { "Date", ".//span[@class='published']" }
                                  }
                              }
                         )
                     }

                 },
                 new Source()
                 {
                     SourceType = SourceType.PageSite,
                     Fields = new SourceFields()
                     {
                         Properties = JsonConvert.SerializeObject
                         (
                            new PageArchitectureSite()
                              {
                                  StartUrl = "https://sverdlovsk.sledcom.ru/Novosti/",
                                  LinkURL = "https://sverdlovsk.sledcom.ru",
                                  EndUrl="/?year=&month=&day=&type=main",
                                  LinkElement = new HtmlElement
                                  {
                                      XPath = @".//div[@class='bl-item-image']/a",
                                      AttributeName = "href"
                                  },

                                  ParseEventProperties = new Dictionary<string, string>
                                  {
                                      { "Body", ".//article[@class='c-detail m_b4']//p" },
                                      { "Date", ".//div[@class='bl-item-date m_b2']" }
                                  }
                              }
                         )
                     }

                 },
                 new Source()
                 {
                     SourceType = SourceType.PageSite,
                     Fields = new SourceFields()
                     {
                         Properties = JsonConvert.SerializeObject
                         (
                            new PageArchitectureSite()
                              {
                                  StartUrl = "https://66.xn--b1aew.xn--p1ai/news/1",
                                  LinkURL = "https://66.xn--b1aew.xn--p1ai",
                                  EndUrl="",
                                  LinkElement = new HtmlElement
                                  {
                                      XPath = @".//div[@class='sl-item-title']/a",
                                      AttributeName = "href"
                                  },

                                  ParseEventProperties = new Dictionary<string, string>
                                  {
                                      { "Body", ".//div[@class='article']//p" },
                                      { "Date", ".//div[@class='article-date-item']" }
                                  }
                              }
                         )
                     }

                 },
                 new Source()
                 {
                     SourceType = SourceType.PageSite,
                     Fields = new SourceFields()
                     {
                         Properties = JsonConvert.SerializeObject
                         (
                            new PageArchitectureSite()
                              {
                                  StartUrl = "https://eburg.mk.ru/news/",
                                  LinkURL = "",
                                  EndUrl="/",
                                  LinkElement = new HtmlElement
                                  {
                                      XPath = @".//a[@class='news-listing__item-link']",
                                      AttributeName = "href"
                                  },

                                  ParseEventProperties = new Dictionary<string, string>
                                  {
                                      { "Body", ".//div[@class='article__body']//p" },
                                      { "Date", ".//time[@class='meta__text']" }
                                  }
                              }
                         )
                     }

                 },

             };*/
            #endregion

        }


    }

   
}
