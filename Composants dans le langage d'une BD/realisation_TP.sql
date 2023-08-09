
create table ouvrages
(
  isbn number(10) not null,
  titre varchar2(200) not null,
  auteur varchar2(80) default null,
  genre char(5) not null, 
  editeur varchar2(80) default null,
  constraint PK_ouvrage_isbn primary key (isbn),
  constraint fk_ouvrages_genres_genre FOREIGN KEY (genre) REFERENCES genres(code) ON DELETE CASCADE
);

create table exemplaires
(
numero NUMBER(3) not null,
isbn number(10) not null,
etat char(5) constraint CK_exemplaires_etat check (etat_exemp in ('NE','BO','MO','MA')),
constraint PK_exemplaires_numero_isbn primary key (numero, isbn),
constraint fk_exemplaires_ouvrages_isbn FOREIGN KEY (isbn) REFERENCES ouvrages(isbn) ON DELETE CASCADE
);
create table genres
(
  code char(5) not null constraint PK_genres_code primary key, 
  libelle varchar2(80) 
);
create table DetailsEmprunts
(
emprunt NUMBER(10) not null, -- contrainte limitation à 5, 2 nouv et 2bd 
numero  number(3) not null, -- clé étrangère, index
isbn number(10),
exemplaire number(3),
rendule date,
constraint PK_detailsemprunts_numero_emprunt primary key (numero, emprunt),
constraint fk_detailsemprunts_emprunts_emprunt FOREIGN KEY (emprunt) REFERENCES Emprunts(numero) ON DELETE CASCADE,
constraint fk_detailsemprunts_exemplaires_isbn FOREIGN KEY (isbn) REFERENCES exemplaires(isbn) ON DELETE CASCADE,
constraint fk_detailsemprunts_exemplaires_exemplaire FOREIGN KEY (exemplaire) REFERENCES exemplaires(numero) ON DELETE CASCADE
);
create table Emprunts
( numero number(10) constraint PK_emprunts_numero primary key, 
  membre number(6),
  creele date default sysdate,
  constraint fk_emprunts_membres FOREIGN KEY (membre) REFERENCES membres(numero) ON DELETE CASCADE
)

-- create index IN_fic_emprunts_num_membre on fic_emprunts(num_membre);

create table membres
(
numero number(6) generated always as identity not null,
nom varchar2(80) not null,
prenom varchar2(80) not null,
adresse varchar2(200),
telephone char(10),
adhesion date not null,
duree number(2) not null constraint CK_membres_duree check (duree >0),
constraint PK_membres_numero primary key (numero)
);
 
------------ sequence pour membre -------------
create sequence seq_membre
  start with 1
  increment by 1
  nocache
  nocycle;
---- 1.4 ----------
alter table membres add
  ( constraint UN_membres unique(nom, prenom, telephone)
);  
---- 1.5 -----
alter table membres add mobile char(10) constraint CK_membres_mobile check (mobile like '06%' or mobile like  '07%'); 
---- 1.6 ---
alter table membres drop constraint un_membres;
alter table membres add constraint un_membres unique(nom, prenom);
alter table membres set unused column telephone ;
alter table membres drop unused columns; 
---- 1.7 ---------
create index IN_genres on ouvrages(genre); 
create index idx_emplaires_isbn on exemplaires(isbn);
create index idx_emprunts_membre on emprunts(membre);
create index idx_details_emprunt on detailsemprunts(emprunt);
create index idx_details_exemplaire on detailsemprunts(isbn, exemplaire);
--- 1.8 ----
alter table DetailsEmprunts drop constraint fk_details_emprunts ;
alter table DetailsEmprunts add constraint fk_details_emprunts foreign key (emprunt) references emprunts(numero) on delete cascade ;
--- 1.9 ---
alter table exemplaires modify etat default 'NE';
--- 1.10 ---
create public synonym abonnes for membres;  

--- 1.11 ---
 rename detailsemprunts to details;

--- 2.1 ---
insert into genres values('REC', 'Récit');
insert into genres values('POL', 'Policier');
insert into genres values ('BD', 'Bande dessinée');
insert into genres values('INF','Informatique');
insert into genres values('THE', 'Théatre');
insert into genres values('ROM', 'Roman');
select * from genres;
commit;

insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2203314168, 'LEFRANC-L''ultimatum', 'Martin, Carin', 'BD', 'Casterman');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2746021285, 'HTML entraînez-vous pour maîtriser le code source', 'Luc Van Lancker', 'INF', 'ENI');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2746026090, ' Oracle 12c SQL, PL/SQL, SQL*Plus', 'J. Gabillaud', 'INF', 'ENI');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2266085816, 'Pantagruel', 'François RABELAIS', 'ROM', 'POCKET');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2266091611, 'Voyage au centre de la terre', 'Jules Verne', 'ROM', 'POCKET');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2253010219, 'Le crime de l''Orient Express', 'Agatha Christie', 'POL', 'Livre de Poche');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2070400816, 'Le Bourgeois gentilhomme', 'Moliere', 'THE', 'Gallimard');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2070367177, 'Le curé de Tours', 'Honoré de Balzac', 'ROM', 'Gallimard');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2080720872, 'Boule de suif', 'Guy de Maupassant', 'REC', 'Flammarion');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2877065073, 'La gloire de mon père', 'Marcel Pagnol', 'ROM', 'Fallois');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2020549522, ' L''aventure des manuscrits de la mer morte ', default, 'REC', 'Seuil');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2253006327, ' Vingt mille lieues sous les mers ', 'Jules Verne', 'ROM', 'LGF');
insert into ouvrages (isbn, titre, auteur, genre, editeur) 
values (2038704015, 'De la terre à la lune', 'Jules Verne', 'ROM', 'Larousse');

insert into exemplaires(isbn, numero, etat) select isbn, 1,'BO' from ouvrages;
insert into exemplaires(isbn, numero, etat) select isbn, 2,'MO' from ouvrages;
delete from exemplaires where isbn=2746021285 and numero=2;
update exemplaires set etat='MO' where isbn=2203314168 and numero=1;
update exemplaires set etat='BO' where isbn=2203314168 and numero=2;
insert into exemplaires(isbn, numero, etat) values (2203314168,3,'NE');
commit; 
--- 2.2 ----
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'ALBERT', 'Anne', '13 rue des alpes', '0601020304', sysdate-60, 1);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'BERNAUD', 'Barnabé', '6 rue des bécasses', '0602030105', sysdate-10, 3);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'CUVARD', 'Camille', '52 rue des cerisiers', '0602010509', sysdate-100, 6);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'DUPOND', 'Daniel', '11 rue des daims', '0610236515', sysdate-250, 12);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'EVROUX', 'Eglantine', '34 rue des elfes', '0658963125', sysdate-150, 6);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'FREGEON', 'Fernand', '11 rue des Francs', '0602036987', sysdate-400, 6);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'GORIT', 'Gaston', '96 rue de la glacerie ', '0684235781', sysdate-150, 1);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'HEVARD', 'Hector', '12 rue haute', '0608546578', sysdate-250, 12);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'INGRAND', 'Irène', '54 rue des iris', '0605020409', sysdate-50, 12);
insert into membres (numero, nom, prenom, adresse, mobile, adhesion, duree) values (seq_membre.nextval, 'JUSTE', 'Julien', '5 place des Jacobins', '0603069876', sysdate-100, 6);

--- 2.3 ----
insert into emprunts(numero, membre, creele) values(1,1,sysdate-200);
insert into emprunts(numero, membre, creele) values(2,3,sysdate-190);
insert into emprunts(numero, membre, creele) values(3,4,sysdate-180);
insert into emprunts(numero, membre, creele) values(4,1,sysdate-170);
insert into emprunts(numero, membre, creele) values(5,5,sysdate-160);
insert into emprunts(numero, membre, creele) values(6,2,sysdate-150);
insert into emprunts(numero, membre, creele) values(7,4,sysdate-140);
insert into emprunts(numero, membre, creele) values(8,1,sysdate-130);
insert into emprunts(numero, membre, creele) values(9,9,sysdate-120);
insert into emprunts(numero, membre, creele) values(10,6,sysdate-110);
insert into emprunts(numero, membre, creele) values(11,1,sysdate-100);
insert into emprunts(numero, membre, creele) values(12,6,sysdate-90);
insert into emprunts(numero, membre, creele) values(13,2,sysdate-80);
insert into emprunts(numero, membre, creele) values(14,4,sysdate-70);
insert into emprunts(numero, membre, creele) values(15,1,sysdate-60);
insert into emprunts(numero, membre, creele) values(16,3,sysdate-50);
insert into emprunts(numero, membre, creele) values(17,1,sysdate-40);
insert into emprunts(numero, membre, creele) values(18,5,sysdate-30);
insert into emprunts(numero, membre, creele) values(19,4,sysdate-20);
insert into emprunts(numero, membre, creele) values(20,1,sysdate-10);


insert into details(emprunt, numero, isbn, exemplaire, rendule) values(1,1,2038704015,1,sysdate-195);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(1,2,2070367177,2,sysdate-190);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(2,1,2080720872,1,sysdate-180);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(2,2,2203314168,1,sysdate-179);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(3,1,2038704015,1,sysdate-170);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(4,1,2203314168,2,sysdate-155);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(4,2,2080720872,1,sysdate-155);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(4,3,2266085816,1,sysdate-159);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(5,1,2038704015,1,sysdate-140);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(6,1,2266085816,2,sysdate-141);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(6,2,2080720872,2,sysdate-130);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(6,3,2746021285,1,sysdate-133);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(7,1,2070367177,2,sysdate-100);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(8,1,2080720872,1,sysdate-116);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(9,1,2038704015,1,sysdate-100);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(10,1,2080720872,2,sysdate-107);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(10,2,2746026090,1,sysdate-78);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(11,1,2746021285,1,sysdate-81);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(12,1,2203314168,1,sysdate-86);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(12,2,2038704015,1,sysdate-60);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(13,1,2070367177,1,sysdate-65);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(14,1,2266091611,1,sysdate-66);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(15,1,2070400816,1,sysdate-50);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(16,1,2253010219,2,sysdate-41);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(16,2,2070367177,2,sysdate-41);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(17,1,2877065073,2,sysdate-36);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(18,1,2070367177,1,sysdate-14);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(19,1,2746026090,1,sysdate-12);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(20,1,2266091611,1,default);
insert into details(emprunt, numero, isbn, exemplaire, rendule) values(20,2,2253010219,1,default);

Insert into ouvrages (isbn, titre, auteur, genre, editeur) values (2080703234, 'Cinq semaines en ballon', 'Jules Verne', 'ROM', 'Flammarion');
--- 2.4 ---
select * from membres;
--- 2.5 ---
alter table membres enable row movement;

--- 2.6 ---
alter table emprunts add etat char(2) default 'EC';
update emprunts set etat ='RE' WHERE etat='EC' and numero not in (select emprunt from details where rendule is null);
--- 2.7 ---
update exemplaires set etat = 'BO' where ( etat='NE' and (select count(*) from details order by numero)>=11) ;
--- 2.10 ---
select * from emprunts where (round((sysdate - creele))<14 ) ;
--- 2.11 ---
