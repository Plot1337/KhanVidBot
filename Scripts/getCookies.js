const printCookies = () => {
  const pairs = document.cookie.split(';');
  let cookies = '';

  for (var i = 0; i < pairs.length; i++) {
    const pair = pairs[i].split('=');
    cookies += pair[0].trim() + '=' + pair[1].trim();

    if (i + 1 < pairs.length) cookies += '; ';
  }

  console.log(cookies);
};

printCookies();
