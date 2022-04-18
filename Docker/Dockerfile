FROM ubuntu:20.04

RUN useradd bot
USER bot

ENV LD_LIBRARY_PATH=/bot/Output
WORKDIR /bot/Output

COPY --chown=bot:bot /play.sh /bot/Output/play.sh
ENTRYPOINT ["/bot/Output/play.sh"]

COPY --chown=bot:bot /Output/ /bot/Output/

