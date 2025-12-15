async function getPersons() {
    const res = await fetch('/api/persons');
    if (!res.ok) { console.error('Fetch error', res.status); return; }
    const persons = await res.json();
    console.log(persons);
    return persons;
}

async function createPerson(person) {
    const res = await fetch('/api/persons', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(person)
    });
    if (!res.ok) { console.error('Fetch POST error', res.status); return; }
    const created = await res.json();
    console.log('Created', created);
    return created;
}

// Expose for console usage
window.PersonsClient = { getPersons, createPerson };
