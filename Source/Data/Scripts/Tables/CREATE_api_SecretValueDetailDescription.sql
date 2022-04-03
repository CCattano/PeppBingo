CREATE TABLE api.SecretValueDetailDescription(
	[Source] varchar(10) NOT NULL,
	[Type] varchar(15) NOT NULL,
	[Value] varchar(50) NOT NULL,
	[Description] varchar(150) NOT NULL,
	CONSTRAINT
		FK_SecretValueDetailDescription_SecretValueDetail_SourceType_SourceType
	FOREIGN KEY
		([Source], [Type])
	REFERENCES
		api.SecretValueDetail([Source], [Type]),
	CONSTRAINT
		UNQ_SecretValueDetailpDescription_Source_Type_Value
	UNIQUE
		(Source, [Type], [Value])
)