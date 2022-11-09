drop database CoalsWebApp;
create database CoalsWebApp;
use CoalsWebApp;
create table users (id int unsigned primary key auto_increment, nickname varchar(25) unique not null,
					login varchar(20) unique not null, email varchar(64) unique not null, password varchar(20) not null,
                    role tinyint unsigned not null);
                    
create table articles (id int unsigned primary key auto_increment, iduser int unsigned not null,
						name nvarchar(30) unique not null, datetimepublish timestamp not null,
                        textarticle text not null, foreign key(iduser) references users(id));
                        
create table comments (id int unsigned primary key auto_increment, idarticle int unsigned not null,
						iduser int unsigned not null, datetimepublish timestamp not null, textcomment text not null,
                        foreign key(idarticle) references articles(id), foreign key(iduser) references users(id));


select * from users;
insert into users(nickname, login, email, password, role) values("Admin", "Admin", "ADMIN@email.com", "ADMIN_1234", 0);
insert into users(nickname, login, email, password, role) values('Authorized', 'Authorized', 'Authorized@email.com', 'Authorized_1234', 2);
insert into users(nickname, login, email, password, role) values('Moderator', 'Moderator', 'Moderator@email.com', 'Moderator_1234', 1);
insert into users(nickname, login, email, password, role) values('Blocked', 'Blocked', 'Blocked@email.com', 'Blocked_1234', 4);

select * from articles;
insert into articles(iduser, name, datetimepublish, textarticle) values(2, 'First article', now(), 'textArticle');
insert into articles(iduser, name, datetimepublish, textarticle) values(2, 'Second article', now(), 'text2Article');
insert into articles(iduser, name, datetimepublish, textarticle) values(2, 'Third article', now(), 'text3Article');

select * from comments;

insert into comments(idarticle, iduser, datetimepublish, textcomment) values(1, 3, now(), 'textComment');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(1, 3, now(), 'textComment_2');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(1, 3, now(), 'textComment_3');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(1, 3, now(), 'textComment_4');

insert into comments(idarticle, iduser, datetimepublish, textcomment) values(3, 2, now(), 'textComment_3');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(3, 2, now(), 'textComment_2_3');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(3, 2, now(), 'textComment_3_3');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(3, 2, now(), 'textComment_4_3');

insert into comments(idarticle, iduser, datetimepublish, textcomment) values(2, 1, now(), 'textComment_2');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(2, 1, now(), 'textComment_2_2');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(2, 1, now(), 'textComment_3_2');
insert into comments(idarticle, iduser, datetimepublish, textcomment) values(3, 1, now(), 'textComment_4_2');
