-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Хост: 192.168.1.10:3306
-- Время создания: Ноя 07 2022 г., 15:14
-- Версия сервера: 5.6.51-log
-- Версия PHP: 7.1.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `finall`
--

-- --------------------------------------------------------

--
-- Структура таблицы `laptop`
--

CREATE TABLE `laptop` (
  `code` int(11) NOT NULL,
  `model` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `speed` smallint(6) NOT NULL,
  `ram` smallint(6) NOT NULL,
  `hd` int(11) NOT NULL,
  `price` double NOT NULL,
  `screen` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `laptop`
--

INSERT INTO `laptop` (`code`, `model`, `speed`, `ram`, `hd`, `price`, `screen`) VALUES
(1, 'Lp_X50', 5600, 8, 250, 1200.55, 14.7),
(2, 'Lp_X60', 7200, 16, 500, 1800.55, 14.7);

-- --------------------------------------------------------

--
-- Структура таблицы `pc`
--

CREATE TABLE `pc` (
  `code` int(11) NOT NULL,
  `model` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `speed` smallint(6) NOT NULL,
  `ram` smallint(6) NOT NULL,
  `hd` int(11) NOT NULL,
  `cd` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `price` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `pc`
--

INSERT INTO `pc` (`code`, `model`, `speed`, `ram`, `hd`, `cd`, `price`) VALUES
(1, 'PC_X30', 3600, 16, 500, 'false', 5530.6),
(2, 'PC_X40', 7200, 64, 2000, 'false', 8530.6);

-- --------------------------------------------------------

--
-- Структура таблицы `printer`
--

CREATE TABLE `printer` (
  `code` int(11) NOT NULL,
  `model` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `color` tinyint(1) NOT NULL,
  `type` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `price` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `printer`
--

INSERT INTO `printer` (`code`, `model`, `color`, `type`, `price`) VALUES
(1, 'Pr_X10', 0, 'Gray', 1000),
(2, 'Pr_X20', 1, 'RGB', 2000);

-- --------------------------------------------------------

--
-- Структура таблицы `product`
--

CREATE TABLE `product` (
  `maker` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `model` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `type` text COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `product`
--

INSERT INTO `product` (`maker`, `model`, `type`) VALUES
('Vitya', 'Lp_X50', 'Laptop'),
('Vitya', 'Lp_X60', 'Laptop'),
('Maks', 'PC_X30', 'PC'),
('Maks', 'PC_X40', 'PC'),
('Petya', 'Pr_X10', 'Printer'),
('Petya', 'Pr_X20', 'Printer');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `laptop`
--
ALTER TABLE `laptop`
  ADD PRIMARY KEY (`code`),
  ADD UNIQUE KEY `model` (`model`);

--
-- Индексы таблицы `pc`
--
ALTER TABLE `pc`
  ADD PRIMARY KEY (`code`),
  ADD UNIQUE KEY `model` (`model`);

--
-- Индексы таблицы `printer`
--
ALTER TABLE `printer`
  ADD PRIMARY KEY (`code`),
  ADD UNIQUE KEY `model` (`model`);

--
-- Индексы таблицы `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`model`);

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `laptop`
--
ALTER TABLE `laptop`
  ADD CONSTRAINT `laptop_ibfk_1` FOREIGN KEY (`model`) REFERENCES `product` (`model`);

--
-- Ограничения внешнего ключа таблицы `pc`
--
ALTER TABLE `pc`
  ADD CONSTRAINT `pc_ibfk_1` FOREIGN KEY (`model`) REFERENCES `product` (`model`);

--
-- Ограничения внешнего ключа таблицы `printer`
--
ALTER TABLE `printer`
  ADD CONSTRAINT `printer_ibfk_1` FOREIGN KEY (`model`) REFERENCES `product` (`model`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
