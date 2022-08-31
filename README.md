# K2-JSReport

Ce connecteur permet de générer un document (PDF, Excel, Word etc...) avec JSReport et de le récupérer dans K2

Le JSON peut etre généré par une procedure stocké, Ex : 
```
-- =============================================

-- Author:		Noham CHOULANT

-- Create date: 11/04/2022

-- Description:	Permet de générer les données JSON

-- =============================================

CREATE PROCEDURE [FNB].[Get_JSON_FNB_PDF] 
	@ID INT
AS
BEGIN
	DECLARE @data NVARCHAR(MAX);


SET @data = (

	SELECT [id]
	,ISNULL(Nom,'')						AS Nom
	,[Duree]
	,(SELECT 
		[MontantAnnuelM2]
		,[MontantAnnuelHT]
		FROM [FNB].[f_Local] as l
		WHERE ID = @ID
		FOR JSON PATH
	)									AS Montant
	FROM [Fiche]	AS f
	WHERE f.id = @ID
	FOR JSON PATH, ROOT('DONNEE')
)

SELECT JSON_QUERY(@data, '$.DONNEE[0]') AS 'Result';

END
```

![Création de l'instance de service](https://zupimages.net/up/22/35/629p.png)


![Utilisation](https://zupimages.net/up/22/35/tiuv.png)


![Le JSON](https://zupimages.net/up/22/35/arjk.png)
