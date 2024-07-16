import json

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


with open('120candidats.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

candidats = []

for feature in data['features']:
    properties = feature['properties']
    circonscription = properties.get("Candidats RN (1) — Feuille 1_Field1", "")
    nom = properties.get("Candidats RN (1) — Feuille 1_Field2", "")
    refs = properties.get("Candidats RN (1) — Feuille 1_Field11", "")
    parsed_refs = None if refs is None else refs.strip().split("\n")

    anecdotes = []
    for i in range(3, 10):
        field = f"Candidats RN (1) — Feuille 1_Field{i}"
        if properties.get(field):
            anecdotes.append(properties[field])

    print(refs)
    candidat = Candidats(circonscription, nom, anecdotes, parsed_refs)
    candidats.append(candidat)

print(f"{len(candidats)} candidats trouve")

candidats_dict_list = [candidat.to_dict() for candidat in candidats]

# Écrire les résultats dans un nouveau fichier JSON
with open('nouveaux_candidats.json', 'w', encoding='utf-8') as f:
    json.dump(candidats_dict_list, f, ensure_ascii=False, indent=4)