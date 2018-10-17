--
-- PostgreSQL database dump
--

-- Dumped from database version 9.6.10
-- Dumped by pg_dump version 9.6.10

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: story_details; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.story_details (
    story_id integer NOT NULL,
    title character varying(200),
    author character varying(100),
    summary character varying(1000),
    characters character varying(100),
    chapters smallint,
    words integer,
    reviews integer,
    favs integer,
    follows integer,
    published date,
    complete boolean,
    url character varying(250)
);


ALTER TABLE public.story_details OWNER TO postgres;

--
-- Name: story_matrix; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.story_matrix (
    story_a integer NOT NULL,
    story_b integer NOT NULL,
    similarity real
);


ALTER TABLE public.story_matrix OWNER TO postgres;

--
-- Name: story_details story_details_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.story_details
    ADD CONSTRAINT story_details_pkey PRIMARY KEY (story_id);


--
-- Name: story_matrix story_matrix_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.story_matrix
    ADD CONSTRAINT story_matrix_pkey PRIMARY KEY (story_a, story_b);


--
-- Name: idx_similarity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_similarity ON public.story_matrix USING btree (similarity);


--
-- Name: idx_story_a; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_story_a ON public.story_matrix USING btree (story_a);


--
-- Name: idx_story_b; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_story_b ON public.story_matrix USING btree (story_b);


--
-- Name: story_matrix fk_story_a; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.story_matrix
    ADD CONSTRAINT fk_story_a FOREIGN KEY (story_a) REFERENCES public.story_details(story_id);


--
-- Name: story_matrix fk_story_b; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.story_matrix
    ADD CONSTRAINT fk_story_b FOREIGN KEY (story_b) REFERENCES public.story_details(story_id);


--
-- Name: TABLE story_details; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT,INSERT,DELETE,TRUNCATE,UPDATE ON TABLE public.story_details TO ffrecs;


--
-- Name: TABLE story_matrix; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT,INSERT,DELETE,TRUNCATE,UPDATE ON TABLE public.story_matrix TO ffrecs;


--
-- PostgreSQL database dump complete
--

