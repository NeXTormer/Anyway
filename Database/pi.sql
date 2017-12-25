use gamedata;
show tables;

insert into games (name) values ("Spacegame");
insert into games (name) values ("Anyway");

select * from games;

desc players;
desc games;
desc scores;

insert into players (name, regdate) values ("holz", now());
insert into players (name, regdate) values ("werner", now());


select * from players;
select * from scores;
select * from games;

insert into scores (score, player_id, game_id) values (14524, 1, 1);
insert into scores (score, player_id, game_id) values (42.5, 3, 4);

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

select score from scores s, players p, games g where s.player_id = p.id and s.game_id = 1 and s.game_id = g.id and p.name = "holz";
select * from scores;
select * from players;