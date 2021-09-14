﻿DROP DATABASE IF EXISTS pony_liga;
CREATE DATABASE IF NOT EXISTS `pony_liga` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `pony_liga`;

CREATE TABLE `apikeys` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `apiKey` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `apikeys` DISABLE KEYS */;
INSERT INTO `apikeys` (`id`, `name`, `apiKey`) VALUES
	(1, 'test', 'df5b0f08-a3ae-4bbc-a26f-42b199de266e');
/*!40000 ALTER TABLE `apikeys` ENABLE KEYS */;

CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `firstName` longtext DEFAULT NULL,
  `surName` longtext DEFAULT NULL,
  `loginName` longtext DEFAULT NULL,
  `passwordHash` longtext DEFAULT NULL,
  `userPrivileges` INT DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `groups` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) DEFAULT NULL,
  `rule` BOOLEAN DEFAULT NULL,
  `groupSize` INTEGER(5) DEFAULT NULL,
  `participants` VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `teams` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `club` VARCHAR(255) DEFAULT NULL,
  `name` VARCHAR(255) DEFAULT NULL,
  `place` INTEGER(11) DEFAULT NULL,
  `consultor` VARCHAR(255) DEFAULT NULL,
  `teamSize` INTEGER(5) DEFAULT NULL,
  `groupId` INTEGER(11) DEFAULT NULL,
  FOREIGN KEY(`groupId`) REFERENCES groups(`id`),
  PRIMARY KEY (`id`),
  UNIQUE (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `teamMembers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `firstName` VARCHAR(255) DEFAULT NULL,
  `surName` VARCHAR(255) DEFAULT NULL,
  `teamId` INTEGER(11) DEFAULT NULL,
  FOREIGN KEY(`teamId`) REFERENCES teams(`id`),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `ponies` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) DEFAULT NULL,
  `race` VARCHAR(255) DEFAULT NULL,
  `age` VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `results` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gameDate` DATE DEFAULT NULL,
  `game` VARCHAR(255) DEFAULT NULL,
  `position` INT(11) DEFAULT NULL,
  `time` VARCHAR(255) DEFAULT NULL,
  `score` INT(11) DEFAULT NULL,
  `teamId` INT(11) DEFAULT NULL,
  FOREIGN KEY(`teamId`) REFERENCES teams(`id`),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;

CREATE TABLE `seasons` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `teamName` VARCHAR(255) DEFAULT NULL,
  `club` VARCHAR(255) DEFAULT NULL,
  `score` INT(11) DEFAULT NULL,
  `year` VARCHAR(10) DEFAULT NULL,
  `placement` INTEGER(5) DEFAULT NULL,
  PRIMARY KEY (`id`)
  ) ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;
  
 CREATE TABLE `teamPonies` (
	`ponyId` int(11) NOT NULL,
	`teamId` int(11) NOT NULL,
	PRIMARY KEY (`teamId`, `ponyId`),
	FOREIGN KEY(`teamId`) REFERENCES teams(`id`),
	FOREIGN KEY(`ponyId`) REFERENCES ponies(`id`)
 )ENGINE=InnoDB DEFAULT CHARSET=UTF8MB4;
  