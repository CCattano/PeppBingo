CREATE TABLE api.Secrets(
	[Source] varchar(10) NOT NULL,
	[Type] varchar(15) NOT NULL,
	[Value] varchar(50) NOT NULL,
	[Description] varchar(150) NOT NULL,
	CONSTRAINT
		UNQ_Secrets_Source_Type_Value
	UNIQUE
		(Source, [Type], [Value])
)