CREATE TABLE token.Secrets(
	[Source] varchar(10) NOT NULL,
	[Type] varchar(15) NOT NULL,
	[Value] varchar(500) NOT NULL,
	[Description] varchar(150) NOT NULL,
	CONSTRAINT
		UNQ_tokenSecrets_Source_Type_Value
	UNIQUE
		(Source, [Type], [Value])
)