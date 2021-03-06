using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;

namespace Fungus
{
    public class ConversationManager
    {
        protected struct ConversationItem
        {
            public string Text { get; set; }
            public Character Character { get; set; }
            public Sprite Portrait { get; set; }
            public RectTransform Position { get; set; }
            public bool Hide { get; set; }
        }

        protected Character[] characters;

        protected bool exitSayWait;

        public void PopulateCharacterCache()
        {
            // cache characters for faster lookup
            characters = UnityEngine.Object.FindObjectsOfType<Character>();
        }

        protected SayDialog GetSayDialog(Character character)
        {
            SayDialog sayDialog = null;
            if (character != null)
            {
                if (character.setSayDialog != null)
                {
                    sayDialog = character.setSayDialog;
                }
            }

            if (sayDialog == null)
            {
                sayDialog = SayDialog.GetSayDialog();
            }

            return sayDialog;
        }

        /// <summary>
        /// Parse and execute a conversation string
        /// </summary>
        /// <param name="conv"></param>
        public IEnumerator DoConversation(string conv)
        {
            if (string.IsNullOrEmpty(conv))
            {
                yield break;
            }
            
            var conversationItems = Parse(conv);

            if (conversationItems.Count == 0)
            {
                yield break;
            }

            // Track the current and previous parameter values
            Character currentCharacter = null;
            Sprite currentPortrait = null;
            RectTransform currentPosition = null;
            Character previousCharacter = null;

            // Play the conversation
            for (int i = 0; i < conversationItems.Count; ++i)
            {
                ConversationItem item = conversationItems[i];
                
                if (item.Character != null)
                {
                    currentCharacter = item.Character;
                }

                currentPortrait = item.Portrait;
                currentPosition = item.Position;

                SayDialog sayDialog = GetSayDialog(currentCharacter);

                if (sayDialog == null)
                {
                    // Should never happen
                    yield break;
                }

                sayDialog.gameObject.SetActive(true);

                if (currentCharacter != null && 
                    currentCharacter != previousCharacter)
                {
                    sayDialog.SetCharacter(currentCharacter);
                }

                var stage = Stage.GetActiveStage();

                if (stage != null && currentCharacter != null &&
                    (currentPortrait != currentCharacter.state.portrait || 
                     currentPosition != currentCharacter.state.position))
                {
                    var portraitOptions = new PortraitOptions(true);
                    portraitOptions.display = item.Hide ? DisplayType.Hide : DisplayType.Show;
                    portraitOptions.character = currentCharacter;
                    portraitOptions.fromPosition = currentCharacter.state.position;
                    portraitOptions.toPosition = currentPosition;
                    portraitOptions.portrait = currentPortrait;

                    // Do a move tween if the character is already on screen and not yet at the specified position
                    if (currentCharacter.state.onScreen &&
                        currentPosition != currentCharacter.state.position)
                    {
                        portraitOptions.move = true;
                    }

                    if (item.Hide)
                    {
                        stage.Hide(portraitOptions);
                    }
                    else
                    {
                        stage.Show(portraitOptions);
                    }
                }

                if (stage == null &&
                    currentPortrait != null)
                {
                    sayDialog.SetCharacterImage(currentPortrait);
                }
                    
                previousCharacter = currentCharacter;
                
                if (!string.IsNullOrEmpty(item.Text)) { 
                    exitSayWait = false;
                    sayDialog.Say(item.Text, true, true, true, false, null, () => {
                        exitSayWait = true;
                    });

                    while (!exitSayWait)
                    {
                        yield return null;
                    }
                    exitSayWait = false;
                }
            }
        }

        protected virtual List<ConversationItem> Parse(string conv)
        {
            //find SimpleScript say strings with portrait options
            //You can test regex matches here: http://regexstorm.net/tester
            var sayRegex = new Regex(@"((?<sayParams>[\w ""]*):)?(?<text>.*)\r*(\n|$)");
            MatchCollection sayMatches = sayRegex.Matches(conv);

            var items = new List<ConversationItem>(sayMatches.Count);

            Character currentCharacter = null;
            for (int i = 0; i < sayMatches.Count; i++)
            {
                string text = sayMatches[i].Groups["text"].Value.Trim();
                string sayParams = sayMatches[i].Groups["sayParams"].Value;

                // As text and SayParams are both optional, an empty string will match the regex.
                // We can ignore any matches where both are empty
                // or if they're Lua style comments
                if ((text.Length == 0 && sayParams.Length == 0) || text.StartsWith("--"))
                {
                    continue;
                }

                string[] separateParams = null;

                if (!string.IsNullOrEmpty(sayParams))
                {
                    separateParams = Split(sayParams);
                }

                var item = CreateConversationItem(separateParams, text, currentCharacter);

                // Previous speaking character is the default for next conversation item
                currentCharacter = item.Character;

                items.Add(item);
            }

            return items;
        }
                                
        /// <summary>
        /// Using the string of say parameters before the ':',
        /// set the current character, position and portrait if provided.
        /// </summary>
        /// <param name="sayParams">The list of say parameters</param>
        protected virtual ConversationItem CreateConversationItem(string[] sayParams, string text, Character currentCharacter)
        {
            var item = new ConversationItem();

            // Populate the story text to be written
            item.Text = text;

            if (sayParams == null || sayParams.Length == 0)
            {
                // Text only, no params - early out.
                return item;
            }

            // try to find the character param first, since we need to get its portrait
            int characterIndex = -1;
            if (characters == null)
            {
                PopulateCharacterCache();
            }

            for (int i = 0; item.Character == null && i < sayParams.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (characters[j].NameStartsWith(sayParams[i]))
                    {
                        characterIndex = i;
                        item.Character = characters[j];
                        break;
                    }
                }
            }

            // Assume last used character if none is specified now
            if (item.Character == null)
            {
                item.Character = currentCharacter;
            }

            // Check if there's a Hide parameter
            int hideIndex = -1;
            if (item.Character != null)
            {
                for (int i = 0; i < sayParams.Length; i++)
                {
                    if (i != characterIndex &&
                        string.Compare(sayParams[i], "hide", true) == 0 )
                    {
                        hideIndex = i;
                        item.Hide = true;
                        break;
                    }
                }
            }
                
            // Next see if we can find a portrait for this character
            int portraitIndex = -1;
            if (item.Character != null)
            {
                for (int i = 0; i < sayParams.Length; i++)
                {
                    if (item.Portrait == null && 
                        item.Character != null &&
                        i != characterIndex && 
                        i != hideIndex) 
                    {
                        Sprite s = item.Character.GetPortrait(sayParams[i]);
                        if (s != null)
                        {
                            portraitIndex = i;
                            item.Portrait = s;
                            break;
                        }
                    }
                }
            }

            // Next check if there's a position parameter
            Stage stage = Stage.GetActiveStage();
            if (stage != null)
            {
                for (int i = 0; i < sayParams.Length; i++)
                {
                    if (i != characterIndex &&
                        i != portraitIndex &&
                        i != hideIndex)
                    {
                        RectTransform r = stage.GetPosition(sayParams[i]);
                        if (r != null)
                        {
                            item.Position = r;
                            break;
                        }
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Splits the string passed in by the delimiters passed in.
        /// Quoted sections are not split, and all tokens have whitespace
        /// trimmed from the start and end.
        protected static string[] Split(string stringToSplit)
        {
            var results = new List<string>();

            bool inQuote = false;
            var currentToken = new StringBuilder();
            for (int index = 0; index < stringToSplit.Length; ++index)
            {
                char currentCharacter = stringToSplit[index];
                if (currentCharacter == '"')
                {
                    // When we see a ", we need to decide whether we are
                    // at the start or send of a quoted section...
                    inQuote = !inQuote;
                }
                else if (char.IsWhiteSpace(currentCharacter) && !inQuote)
                {
                    // We've come to the end of a token, so we find the token,
                    // trim it and add it to the collection of results...
                    string result = currentToken.ToString().Trim( new [] { ' ', '\n', '\t', '\"'} );
                    if (result != "") results.Add(result);

                    // We start a new token...
                    currentToken = new StringBuilder();
                }
                else
                {
                    // We've got a 'normal' character, so we add it to
                    // the curent token...
                    currentToken.Append(currentCharacter);
                }
            }

            // We've come to the end of the string, so we add the last token...
            string lastResult = currentToken.ToString().Trim();
            if (lastResult != "") 
            {
                results.Add(lastResult);
            }

            return results.ToArray();
        }
    }
}