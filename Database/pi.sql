use gamedata;
show tables;

insert into games (name) values ("Spacegame");
insert into games (name) values ("Anyway");

select * from games;

desc users;
desc games;
desc scores;

insert into users (name, regdate, secret) values ("holz", now(), "isdnisuseful");
insert into users (name, regdate, secret) values ("werner", now(), "findenig");


select * from users;

insert into scores (score, user_id, game_id) values (23.3, 2, 4);
insert into scores (score, user_id, game_id) values (42.5, 3, 4);


select * from scores;


select s.score, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "Anyway";

select s.score, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "Spacegame";

select avg(s.score), g.name, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "spacegame" group by u.name;
select max(s.score), g.name, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "spacegame" group by u.name;

select avg(s.score), g.name, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "anyway" group by u.name;
select max(s.score), g.name, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "anyway" group by u.name;

show tables;

use gamedata;