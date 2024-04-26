SELECT U.uni_name AS uni_name, U.uni_location AS uni_location, COUNT(*) as faculties 
FROM Faculties F INNER JOIN Universities U ON F.uni_id = U.uni_id
GROUP BY F.uni_id

SELECT U.uni_name AS uni_name, U.uni_location AS uni_location, t.faculties 
FROM Universities U LEFT JOIN (SELECT uni_id, COUNT(*) as faculties 
FROM Faculties
GROUP BY uni_id) t ON t.uni_id = U.uni_id


insert into Faculties(facult_name, facult_nostud, uni_id) values ('sdrughsrj', 1234, 1),
('sghzsh', 2000, 1), ('whzesfulifszhfszd', 1234, 1), ('sefzzsfsdfszdv', 1234, 2), 
('arweiuyraiwoui', 7800, 2), ('sdrughsrj', 6544, 3), ('hdgndffgn', 1111, 3), 
('strhtdghsd', 1234, 3), ('xcvnsdthh', 1234, 3), ('dfzdbfbzzz', 1234, 4), 
('ufktudtfgfg', 1234, 5), ('cghmdryreter', 1234, 6), ('aergarweghrg', 1234, 1), 
('xgchcgnxcnx', 1234, 1), ('sertgwasgbn', 1234, 3), ('mfdhjtfjhsre', 1234, 4)

delete from Universities where uni_id > 41
select * from Universities