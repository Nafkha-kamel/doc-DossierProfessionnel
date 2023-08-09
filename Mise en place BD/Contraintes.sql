Use Locations
go
 -- BD locations + contraintes --
 alter table FICHES with check
	add constraint FK_FICHES_noCli foreign key (noCli)
	 references CLIENTS(noCli) on delete cascade;

alter table LIGNESFIC with check
	add constraint FK_LIGNESFICHES_noFic foreign key (noFic)
	 references FICHES(noFic) on delete cascade; 


alter table LIGNESFIC with check
	add constraint FK_LIGNESFICHES_refart foreign key (refart)
	 references ARTICLES(refart);

alter table ARTICLES with check
	add constraint FK_ARTICLES_codeGam_codeCate foreign key (codeGam, codeCate)
		references GRILLETARIFS(codeGam, codeCate);
alter table GRILLETARIFS with check add
	    constraint FK_GRILLETARIFS_codeGam foreign key (codeGam)
		references GAMMES(codeGam),
	    constraint FK_GRILLETARIFS_codecate foreign key (codeCate)
		references CATEGORIES(codeCate),
	    constraint FK_GRILLETARIFS_codeTarif foreign key (codeTarif)
		references TARIFS(codeTarif);

/*create nonclustered index FK_FICHES_noCli on FICHES(noCli asc) ;
create nonclustered index FK_LIGNESFIC_noFic on LIGNESFIC(noFic asc) ;
create nonclustered index FK_LIGNESFIC_noFic on LIGNESFIC(noFic asc) ;



create nonclustered index FK_Employes8Employes on Employes(CodeChef asc);
create nonclustered index FK_Conges_Employes on Conges(CodeEmp asc);
create nonclustered index FK_Conges_mens_Conges on Conges_Mens(CodeEmp asc, Annee asc);*/
