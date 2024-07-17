import json
import sys

class Candidats:
    def __init__(self, circonscription, nom, anecdotes, refs):
        self.circonscription = circonscription
        self.nom = nom
        self.anecdotes = anecdotes
        self.refs = refs

    def __repr__(self):
        return f"Candidats(circonscription={self.circonscription}, nom={self.nom}, anecdotes={self.anecdotes}, refs={self.refs})"

    def to_dict(self):
        return {
            'nom': self.nom,
            'circonscription': self.circonscription,
            'anecdotes': self.anecdotes,
            'refs': self.refs
        }

input_file = sys.argv[1]

with open(input_file, 'r', encoding='utf-8') as f:
    data = json.load(f)

candidats = []

for feature in data['features']:
    properties = feature['properties']

    if properties.get("elus final2 — Feuille 1_Field1", "") == "ÉLIMINÉ(E)":
        continue

    circonscription = properties.get("elus final2 — Feuille 1_Field2", "")
    nom = properties.get("elus final2 — Feuille 1_Field4", "")
    refs = properties.get("elus final2 — Feuille 1_Field12", "")
    parsed_refs = None if refs is None else refs.strip().split("\n")

    anecdotes = []
    for i in range(5, 11):
        field = f"elus final2 — Feuille 1_Field{i}"
        if properties.get(field):
            anecdotes.append(properties[field])

    candidat = Candidats(circonscription, nom, anecdotes, parsed_refs)
    candidats.append(candidat)

print(f"{len(candidats)} candidats trouve")

candidats_dict_list = [candidat.to_dict() for candidat in candidats]

# Écrire les résultats dans un nouveau fichier JSON
with open('nouveaux_candidats.json', 'w', encoding='utf-8') as f:
    json.dump(candidats_dict_list, f, ensure_ascii=False, indent=4)