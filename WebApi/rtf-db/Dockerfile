FROM mongo:latest

COPY rtf-db.instructionStep.json /rtf-db.instructionStep.json
COPY rtf-db.labReservations.json /rtf-db.labReservations.json
COPY rtf-db.labs.json /rtf-db.labs.json
COPY rtf-db.labsEntity.json /rtf-db.labsEntity.json
COPY rtf-db.news.json /rtf-db.news.json
COPY rtf-db.labWorkInstruction.json /rtf-db.labWorkInstruction.json
COPY rtf-db.labWorks.json /rtf-db.labWorks.json
COPY rtf-db.userLabResult.json /rtf-db.userLabResult.json
COPY rtf-db.users.json /rtf-db.users.json
COPY rtf-db.vms.json /rtf-db.vms.json

CMD mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection instructionStep --type json --file /rtf-db.instructionStep.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection labReservations --type json --file /rtf-db.labReservations.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection labs --type json --file /rtf-db.labs.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection labsEntity --type json --file /rtf-db.labsEntity.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection news --type json --file /rtf-db.news.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection labWorkInstruction --type json --file /rtf-db.labWorkInstruction.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection labWorks --type json --file /rtf-db.labWorks.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection userLabResult --type json --file /rtf-db.userLabResult.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection users --type json --file /rtf-db.users.json --jsonArray && \
     mongoimport --uri mongodb://mongo:27017 --db rtf-db --collection vms --type json --file /rtf-db.vms.json --jsonArray