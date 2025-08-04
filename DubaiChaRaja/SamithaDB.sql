--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-08-04 08:22:01

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE samitha;
--
-- TOC entry 4923 (class 1262 OID 16529)
-- Name: samitha; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE samitha WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en-US';


\connect samitha

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 232 (class 1255 OID 16603)
-- Name: deletefestivalrecord(integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.deletefestivalrecord(input_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM FestivalRecords WHERE Id = input_id;
END;
$$;


--
-- TOC entry 233 (class 1255 OID 16598)
-- Name: deletefestivalrecord(integer, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.deletefestivalrecord(input_id integer, input_userid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM FestivalRecords WHERE Id = input_id AND UserId = input_userid;
END;
$$;


--
-- TOC entry 229 (class 1255 OID 16599)
-- Name: getallfestivalrecords(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.getallfestivalrecords() RETURNS TABLE(id integer, description text, amount numeric, type text, year integer, createdat timestamp without time zone, userid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM FestivalRecords ORDER BY Year DESC, CreatedAt DESC;
END;
$$;


--
-- TOC entry 234 (class 1255 OID 16604)
-- Name: getfestivalrecordsbyyear(integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.getfestivalrecordsbyyear(input_year integer) RETURNS TABLE(id integer, description text, amount numeric, type text, year integer, createdat timestamp without time zone, userid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM FestivalRecords WHERE Year = input_year;
END;
$$;


--
-- TOC entry 230 (class 1255 OID 16600)
-- Name: getfestivalrecordsbyyear(integer, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.getfestivalrecordsbyyear(input_year integer, input_userid integer) RETURNS TABLE(id integer, description text, amount numeric, type text, year integer, createdat timestamp without time zone, userid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM FestivalRecords WHERE Year = input_year AND UserId = input_userid;
END;
$$;


--
-- TOC entry 235 (class 1255 OID 16605)
-- Name: insertfestivalrecord(text, numeric, text, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.insertfestivalrecord(input_description text, input_amount numeric, input_type text, input_year integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO FestivalRecords (Description, Amount, Type, Year, CreatedAt)
    VALUES (input_description, input_amount, input_type, input_year, NOW());
END;
$$;


--
-- TOC entry 236 (class 1255 OID 16601)
-- Name: insertfestivalrecord(text, numeric, text, integer, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.insertfestivalrecord(input_description text, input_amount numeric, input_type text, input_year integer, input_userid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO FestivalRecords (Description, Amount, Type, Year, CreatedAt, UserId)
    VALUES (input_description, input_amount, input_type, input_year, NOW(), input_userid);
END;
$$;


--
-- TOC entry 231 (class 1255 OID 16602)
-- Name: updatefestivalrecord(integer, text, numeric, text, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.updatefestivalrecord(input_id integer, input_description text, input_amount numeric, input_type text, input_year integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE FestivalRecords
    SET Description = input_description,
        Amount = input_amount,
        Type = input_type,
        Year = input_year
    WHERE Id = input_id;
END;
$$;


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16536)
-- Name: ExpenseRecords; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."ExpenseRecords" (
    "Id" integer NOT NULL,
    "Year" integer NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Type" text NOT NULL,
    "Description" text NOT NULL,
    "Amount" numeric NOT NULL
);


--
-- TOC entry 218 (class 1259 OID 16535)
-- Name: ExpenseRecords_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."ExpenseRecords" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."ExpenseRecords_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 221 (class 1259 OID 16544)
-- Name: User; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."User" (
    "Id" integer NOT NULL,
    "Username" text NOT NULL,
    "Password" text NOT NULL
);


--
-- TOC entry 220 (class 1259 OID 16543)
-- Name: User_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."User" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."User_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 217 (class 1259 OID 16530)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


--
-- TOC entry 223 (class 1259 OID 16552)
-- Name: festivalrecords; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.festivalrecords (
    id integer NOT NULL,
    description text NOT NULL,
    amount numeric(18,2) NOT NULL,
    type text NOT NULL,
    year integer NOT NULL,
    createdat timestamp without time zone DEFAULT now() NOT NULL,
    userid integer
);


--
-- TOC entry 222 (class 1259 OID 16551)
-- Name: festivalrecords_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.festivalrecords_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4924 (class 0 OID 0)
-- Dependencies: 222
-- Name: festivalrecords_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.festivalrecords_id_seq OWNED BY public.festivalrecords.id;


--
-- TOC entry 224 (class 1259 OID 16561)
-- Name: pendingverifications; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.pendingverifications (
    email character varying(255) NOT NULL,
    code character varying(10),
    expiry timestamp without time zone DEFAULT (now() + '00:10:00'::interval)
);


--
-- TOC entry 226 (class 1259 OID 16568)
-- Name: userimages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.userimages (
    id integer NOT NULL,
    userid integer NOT NULL,
    filename character varying(255),
    uploaddate timestamp without time zone DEFAULT now()
);


--
-- TOC entry 225 (class 1259 OID 16567)
-- Name: userimages_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.userimages_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4925 (class 0 OID 0)
-- Dependencies: 225
-- Name: userimages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.userimages_id_seq OWNED BY public.userimages.id;


--
-- TOC entry 228 (class 1259 OID 16576)
-- Name: users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(100) NOT NULL,
    passwordhash character varying(200) NOT NULL,
    isadmin boolean DEFAULT false NOT NULL,
    email character varying(150) DEFAULT 'user@example.com'::character varying NOT NULL,
    hasaccess boolean DEFAULT false,
    access_requested boolean DEFAULT false,
    isverified boolean DEFAULT false
);


--
-- TOC entry 227 (class 1259 OID 16575)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4926 (class 0 OID 0)
-- Dependencies: 227
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4731 (class 2604 OID 16555)
-- Name: festivalrecords id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.festivalrecords ALTER COLUMN id SET DEFAULT nextval('public.festivalrecords_id_seq'::regclass);


--
-- TOC entry 4734 (class 2604 OID 16571)
-- Name: userimages id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.userimages ALTER COLUMN id SET DEFAULT nextval('public.userimages_id_seq'::regclass);


--
-- TOC entry 4736 (class 2604 OID 16579)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 4908 (class 0 OID 16536)
-- Dependencies: 219
-- Data for Name: ExpenseRecords; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 4910 (class 0 OID 16544)
-- Dependencies: 221
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."User" VALUES (1, 'admin', 'admin123');
INSERT INTO public."User" VALUES (2, 'admin', 'admin123');


--
-- TOC entry 4906 (class 0 OID 16530)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20250521045101_InitialCreate', '9.0.5');


--
-- TOC entry 4912 (class 0 OID 16552)
-- Dependencies: 223
-- Data for Name: festivalrecords; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.festivalrecords VALUES (29, 'chota murti ', 5000.00, 'Expense', 2025, '2025-07-24 13:08:20.890002', 3);
INSERT INTO public.festivalrecords VALUES (31, 'pratham ', 10000.00, 'Donation', 2025, '2025-07-24 13:30:08.634781', 1);


--
-- TOC entry 4913 (class 0 OID 16561)
-- Dependencies: 224
-- Data for Name: pendingverifications; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.pendingverifications VALUES ('prathamshetty318@gmail.com', '564369', '2025-07-17 19:03:54.32498');
INSERT INTO public.pendingverifications VALUES ('shettypratham73@gmail.com', '862883', '2025-07-19 09:20:05.056248');
INSERT INTO public.pendingverifications VALUES ('monkey@gmail.com', '802610', '2025-07-19 09:55:55.840184');


--
-- TOC entry 4915 (class 0 OID 16568)
-- Dependencies: 226
-- Data for Name: userimages; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 4917 (class 0 OID 16576)
-- Dependencies: 228
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.users VALUES (3, 'moba2', 'D/4avRoIIVNTwjPW4AlhPpXuxCU4Mqdhryj/N6xaFQw=', false, 'monkey@gmail.com', false, false, false);
INSERT INTO public.users VALUES (2, 'moba1', 'FeKw08M4keuw8e9gnsQZQgwg4yDOlMZfvIwzEkSOsiU=', true, 'prathamshetty318@gmail.com', false, false, false);
INSERT INTO public.users VALUES (1, 'moba', 'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', false, 'shettypratham73@gmail.com', true, false, false);


--
-- TOC entry 4927 (class 0 OID 0)
-- Dependencies: 218
-- Name: ExpenseRecords_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."ExpenseRecords_Id_seq"', 1, false);


--
-- TOC entry 4928 (class 0 OID 0)
-- Dependencies: 220
-- Name: User_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."User_Id_seq"', 2, true);


--
-- TOC entry 4929 (class 0 OID 0)
-- Dependencies: 222
-- Name: festivalrecords_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.festivalrecords_id_seq', 31, true);


--
-- TOC entry 4930 (class 0 OID 0)
-- Dependencies: 225
-- Name: userimages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.userimages_id_seq', 6, true);


--
-- TOC entry 4931 (class 0 OID 0)
-- Dependencies: 227
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.users_id_seq', 3, true);


--
-- TOC entry 4745 (class 2606 OID 16542)
-- Name: ExpenseRecords PK_ExpenseRecords; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ExpenseRecords"
    ADD CONSTRAINT "PK_ExpenseRecords" PRIMARY KEY ("Id");


--
-- TOC entry 4747 (class 2606 OID 16550)
-- Name: User PK_User; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "PK_User" PRIMARY KEY ("Id");


--
-- TOC entry 4743 (class 2606 OID 16534)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4749 (class 2606 OID 16560)
-- Name: festivalrecords festivalrecords_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.festivalrecords
    ADD CONSTRAINT festivalrecords_pkey PRIMARY KEY (id);


--
-- TOC entry 4752 (class 2606 OID 16566)
-- Name: pendingverifications pendingverifications_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.pendingverifications
    ADD CONSTRAINT pendingverifications_pkey PRIMARY KEY (email);


--
-- TOC entry 4754 (class 2606 OID 16574)
-- Name: userimages userimages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.userimages
    ADD CONSTRAINT userimages_pkey PRIMARY KEY (id);


--
-- TOC entry 4756 (class 2606 OID 16584)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4758 (class 2606 OID 16586)
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- TOC entry 4750 (class 1259 OID 16597)
-- Name: ix_festivalrecords_userid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX ix_festivalrecords_userid ON public.festivalrecords USING btree (userid);


--
-- TOC entry 4759 (class 2606 OID 16592)
-- Name: festivalrecords fk_festivalrecords_users; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.festivalrecords
    ADD CONSTRAINT fk_festivalrecords_users FOREIGN KEY (userid) REFERENCES public.users(id);


--
-- TOC entry 4760 (class 2606 OID 16587)
-- Name: userimages fk_userimages_users; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.userimages
    ADD CONSTRAINT fk_userimages_users FOREIGN KEY (userid) REFERENCES public.users(id);


-- Completed on 2025-08-04 08:22:02

--
-- PostgreSQL database dump complete
--

