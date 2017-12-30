use gamedata;
show tables;

insert into games (name) values ("Spacegame");
insert into games (name) values ("Anyway");

select * from games;

desc players;
desc games;
desc scores;

alter table scores add date datetime;

insert into players (name, regdate) values ("holz", now());
insert into players (name, regdate) values ("werner", now());


select * from players;
select * from scores;
select * from games;

insert into scores (score, player_id, game_id) values (14524, 1, 1);
insert into scores (score, player_id, game_id) values (42.5, 3, 2);

INSERT INTO scores (score, player_id, game_id) VALUES (?, ?, ?);

select * from scores;

select * from players;


select s.score, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "Anyway";

select s.score, u.name from scores s, users u, games g where s.game_id = g.id and s.user_id = u.id and g.name = "Spacegame";

select avg(s.score), g.name, u.name from scores s, players u, games g where s.game_id = g.id and s.player_id = u.id and g.name = "spacegame" group by u.name;
select max(s.score), g.name, u.name from scores s, players u, games g where s.game_id = g.id and s.player_id = u.id and g.name = "spacegame" group by u.name;

select avg(s.score), g.name, u.name from scores s, players u, games g where s.game_id = g.id and s.player_id = u.id and g.name = "anyway" group by u.name;
select max(s.score), g.name, u.name from scores s, players u, games g where s.game_id = g.id and s.player_id = u.id and g.name = "anyway" group by u.name;

show tables;

SELECT id from games where name = "spacegame";
SELECT id FROM players WHERE name = "holz";

use gamedata;

select p.name, score, s.date from scores s, players p, games g where s.player_id = p.id and s.game_id = 1 and s.game_id = g.id and p.name = "schubwerner";
select * from scores;
select * from players;


select max(score), p.name, s.date from players p, games g, scores s where g.name = 'spacegame' and s.game_id = g.id and s.player_id = p.id group by p.name order by score desc;

SELECT p.name, s.score, s.date FROM players p, games g, scores s WHERE g.name = "spacegame" AND s.game_id = g.id AND s.player_id = p.id ORDER BY score DESC;



desc scores;


SELECT id FROM games WHERE name = "spacegame";

INSERT INTO scores (score, player_id, game_id, date) VALUES (2323, 6, 2, now());











