# York University Course Scraper
Small program I made to scrape all active courses currently at York University in Toronto, ON, Canada.

Works by opening the webpages containing all currently active courses for a specific faculty then going through the table on that page row by row to get data for each course. It doesn't get extra info like sections or instructors, just basic course info that can be used to sort courses or filter courses. It then inserts each course into a database so that it may be used by my Discord bot, Forkbot, the bot used by the York University discords.

Feel free to use it in any way you like!
Or, if you'd prefer, there's a comma separated values file (Course List.csv) that contains all the active courses as of January 17th 2020.

## Packages Used
  - [Html Agility Pack](https://html-agility-pack.net/)
  - [System.Data.SQLite](https://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki)
