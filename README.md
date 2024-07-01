# RIA_OOP
Курсовой проект по "Основам объектно-ориентированного программирования"
Вариант No 24 (ДТП)
Программная система, предназначенную для анализа
дорожно-транспортных происшествий (ДТП). Такая система должна обеспечивать
хранение сведений о ДТП. Для каждого ДТП должны быть сохранены: вид ДТП (наезд на
пешехода, наезд на препятствие, столкновение, опрокидывание и т. д.), дата, гос. номера
автомобилей (если в ДТП участвовали несколько автомобилей), данные о водителе и
причина (выезд на полосу встречного движения, состояние водителя, неисправность
автомобиля, нарушение ПДД и т. д.).

Целью курсовой работы является разработка в среде Visual Studio приложения с
графическим интерфейсом для работы с локальными обобщенными списками.
Информация для списков сохраняется в бинарных файлах, с использованием
сериализации.

Приложение обрабатывает шесть запросов.
• Список водителей, совершивших более одного ДТП.
• Список водителей, участвующих в ДТП в заданном месте.
• Список водителей, участвующих в ДТП на заданную дату.
• ДТП с максимальным количеством потерпевших.
• Список водителей, участвующих в ДТП с наездом на пешеходов.
• Причины ДТП в порядке убывания их количества.
Запросы выполняются в форме LINQ to Objects. На главной форме приложения
располагаться меню для сохранения изменений, выполнения запросов, выхода из
приложения. Кроме того, на главной форме отображаются данные всех таблиц базы
данных в отдельных вкладках, предусмотрена возможность добавлять и
удалять записи. Результаты запросов отображаются в отдельных окнах.

